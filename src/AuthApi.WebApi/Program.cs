using AuthApi.Application.Infrastructure.Security.JWT;
using AuthApi.WebApi.IoC;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationConfigurations();
builder.Services.AddControllerConfigurations();
builder.Services.AddMongoDbConfigurations(builder.Configuration);
builder.Services.AddJwtConfigurations(builder.Configuration["JwtPrivateKey"]);
builder.Services.AddOpenTelemetryConfigurations();

builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot"))
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
