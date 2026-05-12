using LMS.Common.Exceptions;
using LMS.Common.Responses;
using System.Net;
using System.Text.Json;

namespace LMS.Enrollment.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && !context.Response.HasStarted)
                {
                    await HandleResponseAsync(context, (int)HttpStatusCode.NotFound, "The requested endpoint was not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error : {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleResponseAsync(HttpContext context, int statusCode, string msg)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ApiResponse<object>(false, null, msg, statusCode);

            var json = JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );

            await context.Response.WriteAsync(json);
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            string serverErrorMessage = _env.IsDevelopment() ?
                $"{ex.Message} \n {ex.StackTrace}" : "An unexpected error occurred. Please try again later.";

            int statusCode = ex switch
            {
                DomainNotFoundException => (int)HttpStatusCode.NotFound,
                DomainUnauthorizedException => (int)HttpStatusCode.Unauthorized,
                DomainValidationException => (int)HttpStatusCode.BadRequest,
                DomainConflictException => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            string message = statusCode == (int)HttpStatusCode.InternalServerError ? serverErrorMessage : ex.Message;

            var response = new ApiResponse<object>(false, null, message, statusCode);

            var json = JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );

            await context.Response.WriteAsync(json);
        }
    }
}
