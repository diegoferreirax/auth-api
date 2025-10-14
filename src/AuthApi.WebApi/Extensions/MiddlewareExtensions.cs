using AuthApi.WebApi.Middleware;

namespace AuthApi.WebApi.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder
            .UseMiddleware<ErrorHandlingMiddleware>()
            .UseMiddleware<ValidationErrorHandlingMiddleware>();
    }
}
