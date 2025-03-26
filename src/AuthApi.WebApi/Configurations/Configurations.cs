using AuthApi.Application.Features.Users;
using AuthApi.Application.Features.Users.RegisterUser.v1;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.WebApi.Configurations;

public static class Configurations
{
    public static IServiceCollection AddCustomConfigurations(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<RegisterUserHandler>();
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
