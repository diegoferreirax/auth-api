using AuthApi.Application.Security.JWT;
using AuthApi.WebApi.IoC;
using AuthApi.WebApi.Extensions;
using AuthApi.WebApi.Filters;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var jwtPrivateKey = builder.Configuration["JwtPrivateKey"];
if (string.IsNullOrEmpty(jwtPrivateKey))
{
    throw new InvalidOperationException("JwtPrivateKey configuration is missing or null.");
}

builder.Services.AddApplicationConfigurations();
builder.Services.AddControllerConfigurations();
builder.Services.AddJwtConfigurations(jwtPrivateKey);
builder.Services.AddOpenTelemetryConfigurations();
builder.Services.AddBCryptConfigurations();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddSwaggerConfigurations();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationActionFilter>();
});

builder.Logging.AddConsole();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseErrorHandling();
app.MapHealthChecks("/health");

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger(c =>
{
    c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API v1.0");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Auth API Documentation";
});
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
