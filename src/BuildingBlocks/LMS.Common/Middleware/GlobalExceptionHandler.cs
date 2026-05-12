using System;
using LMS.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LMS.Common.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request.");

        var (statusCode, title) = exception switch
        {
            DomainNotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),
            DomainConflictException => (StatusCodes.Status409Conflict, "Conflict"),
            DomainUnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            DomainValidationException => (StatusCodes.Status503ServiceUnavailable, "Validation Error"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
