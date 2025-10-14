using AuthApi.Application.Models;
using System.Net;
using System.Text.Json;

namespace AuthApi.WebApi.Middleware;

public class ValidationErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationErrorHandlingMiddleware> _logger;

    public ValidationErrorHandlingMiddleware(RequestDelegate next, ILogger<ValidationErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest && 
            context.Items.ContainsKey("ValidationErrors"))
        {
            await HandleValidationErrorsAsync(context);
        }
    }

    private static async Task HandleValidationErrorsAsync(HttpContext context)
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

            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
