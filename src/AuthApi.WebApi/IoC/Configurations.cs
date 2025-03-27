using AuthApi.Application.Features.Users;
using AuthApi.Application.Features.Users.AuthenticateUser.v1;
using AuthApi.Application.Features.Users.RegisterUser.v1;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.WebApi.IoC;

public static class Configurations
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<AuthenticateUserHandler>();
        services.AddControllers().AddControllersAsServices();

        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

        return services;
    }
}
