using AuthApi.Application.Exceptions;
using System.Net;
using System.Text.Json;
using AuthApi.Application.Common.Responses;

namespace AuthApi.WebApi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var errorResponse = exception switch
        {
            ValidationException validationEx => CreateValidationErrorResponse(
                validationEx.Message, 
                validationEx.Errors),

            BusinessException businessEx => CreateErrorResponse(
                businessEx.Message, 
                null, 
                (HttpStatusCode)businessEx.StatusCode),
            
            ArgumentException => CreateErrorResponse(
                "Invalid argument provided", 
                exception.Message, 
                HttpStatusCode.BadRequest),
            
            UnauthorizedAccessException => CreateErrorResponse(
                "Unauthorized access", 
                "You do not have permission to access this resource", 
                HttpStatusCode.Unauthorized),
            
            KeyNotFoundException => CreateErrorResponse(
                "Resource not found", 
                exception.Message, 
                HttpStatusCode.NotFound),
            
            InvalidOperationException => CreateErrorResponse(
                "Invalid operation", 
                exception.Message, 
                HttpStatusCode.BadRequest),
            
            TimeoutException => CreateErrorResponse(
                "Request timeout", 
                "The request took too long to process", 
                HttpStatusCode.RequestTimeout),
            
            _ => CreateErrorResponse(
                "An unexpected error occurred", 
                "Please try again later", 
                HttpStatusCode.InternalServerError)
        };

        errorResponse.TraceId = context.TraceIdentifier;
        context.Response.StatusCode = (int)errorResponse.StatusCode;

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private static ErrorResponse CreateErrorResponse(string message, string? details, HttpStatusCode statusCode)
    {
        return new ErrorResponse
        {
            Message = message,
            Details = details,
            StatusCode = (int)statusCode
        };
    }

    private static ValidationErrorResponse CreateValidationErrorResponse(string message, Dictionary<string, string[]> errors)
    {
        return new ValidationErrorResponse
        {
            Message = message,
            StatusCode = 400,
            Errors = errors
        };
    }
}
