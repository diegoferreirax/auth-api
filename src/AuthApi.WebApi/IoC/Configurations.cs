using AuthApi.Application.Features.Users;
using AuthApi.Application.Features.Users.AuthenticateUser.v1;
using AuthApi.Application.Features.Users.RegisterUser.v1;
using AuthApi.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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

    public static IServiceCollection AddMongoDbConfigurations(this IServiceCollection services, ConfigurationManager configuration)
    {
        var section = configuration.GetSection("AuthDatabase");
        services.Configure<MongoDBDatabaseSettings>(section);

        services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(section["ConnectionString"]));
        services.AddSingleton(typeof(MongoDBDatabaseConfig<>));
        MongoDBDatabaseMapper.RegisterMappings();
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
}
