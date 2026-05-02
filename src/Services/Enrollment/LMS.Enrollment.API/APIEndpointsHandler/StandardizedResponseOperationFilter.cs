using LMS.Common.Responses;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace LMS.Enrollment.API.APIEndpointsHandler
{
    public class StandardizedResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // we want to generate the schema for ApiResponse<object> and use it in the OpenApiResponse
            var errorSchema = context.SchemaGenerator.GenerateSchema(typeof(ApiResponse<object>), context.SchemaRepository);

            var errorResponse = new OpenApiResponse
            {
                Description = "Standardized Error Response",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType { Schema = errorSchema }
                }
            };

            // in the operation, we want to add the errorResponse for the following status codes: 400, 401, 404, 409, 500
            operation.Responses.TryAdd("400", errorResponse);
            operation.Responses.TryAdd("401", errorResponse);
            operation.Responses.TryAdd("404", errorResponse);
            operation.Responses.TryAdd("409", errorResponse);
            operation.Responses.TryAdd("500", errorResponse);
        }
    }
}
