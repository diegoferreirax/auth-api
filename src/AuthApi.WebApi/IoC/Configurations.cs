using AuthApi.Application.Features.Users.Queries.ListUsers;
using AuthApi.Application.Persistence.Context;
using AuthApi.Application.Persistence.Repositories;
using AuthApi.Application.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.OpenApi.Models;
using AuthApi.Application.Common.Security.Bcrypt;
using AuthApi.Application.Features.Users.Commands.AuthenticateUser.v1;
using AuthApi.Application.Features.Users.Commands.RegisterUser.v1;
using AuthApi.Application.Features.Users.Commands.UpdateUser.v1;
using AuthApi.Application.Features.Users.Commands.DeleteUser.v1;

namespace AuthApi.WebApi.IoC;

public static class Configurations
{
    public static IServiceCollection AddApplicationConfigurations(this IServiceCollection services)
    {
        services.AddScoped<UserRoleRepository>();
        services.AddScoped<RoleRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<AuthenticateUserHandler>();
        services.AddScoped<DeleteUserHandler>();
        services.AddScoped<ListUsersHandler>();
        services.AddScoped<UpdateUserHandler>();

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

    public static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Auth API",
                Version = "v1.0",
                Description = "API de autenticação e gerenciamento de usuários",
                Contact = new OpenApiContact
                {
                    Name = "Auth API Team"
                }
            });
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            c.TagActionsBy(api => new[] { "Users" });
        });

        return services;
    }
}
