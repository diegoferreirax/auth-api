using AuthApi.Application.Features.Users;
using AuthApi.Application.Features.Users.AuthenticateUser.v1;
using AuthApi.Application.Features.Users.DeleteUser.v1;
using AuthApi.Application.Features.Users.RegisterUser.v1;
using AuthApi.Application.Infrastructure.Data;
using AuthApi.Application.Infrastructure.Security.Bcrypt;
using AuthApi.Application.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace AuthApi.WebApi.IoC;

public static class Configurations
{
    public static IServiceCollection AddApplicationConfigurations(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<AuthenticateUserHandler>();
        services.AddScoped<DeleteUserHandler>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddControllerConfigurations(this IServiceCollection services)
    {
        services.AddControllers().AddControllersAsServices();
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });
        return services;
    }

    public static IServiceCollection AddOpenTelemetryConfigurations(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(t =>
            {
                t.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter();
            })
            .WithMetrics(m =>
            {
                m.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter();
            });
        return services;
    }

    public static IServiceCollection AddBCryptConfigurations(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetValue<string>("AuthDbConnectionString");

        services.AddDbContext<AuthDbContext>(options =>
            options.UseMySql(connectionString,
            new MySqlServerVersion(new Version(8, 0, 21)),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                ))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableDetailedErrors()
                .UseUpperSnakeCaseNamingConvention()
        );

        return services;
    }
}
