using System.Net;
using System.Text.Json;
using AuthApi.Application.Common.Responses;

namespace AuthApi.WebApi.Middleware;

public class ValidationErrorHandlingMiddleware(RequestDelegate next, ILogger<ValidationErrorHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ValidationErrorHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest && 
            context.Items.ContainsKey("ValidationErrors"))
        {
            await HandleValidationErrorsAsync(context, GetOptions());
        }
    }

    private static JsonSerializerOptions GetOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private static async Task HandleValidationErrorsAsync(HttpContext context, JsonSerializerOptions options)
    {
        context.Response.ContentType = "application/json";
        
        if (context.Items["ValidationErrors"] is Dictionary<string, string[]> validationErrors)
        {
            var errorResponse = new ValidationErrorResponse
            {
                Message = "Validation failed",
                StatusCode = (int)HttpStatusCode.BadRequest,
                TraceId = context.TraceIdentifier,
                Errors = validationErrors
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse, options);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
