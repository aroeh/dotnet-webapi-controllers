using Demo.Restuarants.API.Middleware;
using Demo.Restuarants.Core.Extensions;
using Demo.Restuarants.Infrastructure.MongoDb.Extensions;
using Demo.Restuarants.Infrastructure.MongoDb.Health;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Read appsettings files
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");

// Add services to the container.

//TODO: Figure out how to get reading from Azure App Config working
//string connectionString = builder.Configuration.GetConnectionString("AppConfig");
//builder.Configuration.AddAzureAppConfiguration(connectionString);
//builder.Services.Configure<>(builder.Configuration.GetSection("TestApp:Settings"));

// configure authoriation and authentication for the API with Microsoft EntraID
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// register a custom exception handler middleware and problem details usage in the pipeline
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// add health checks to the app
builder.Services.AddHealthChecks()
    .AddCheck<CustomMongoDbHealthCheck>("Database"); // this demonstrates using a custom mongodb health check

builder.Services.AddControllers()
    .AddJsonOptions(options => // handle enum json serialization for incoming requests
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// handle enum json serialization for responses
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// setup api versioning using query string versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCoreOrchestrations();
builder.Services.AddInfrastructureRepos(builder.Configuration);

// add hsts security headers
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// use the global exception handler
app.UseExceptionHandler();

app.UseHttpsRedirection();

// map the health check endpoint and setup a custom writer
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = HealthCheckResponseWriter.WriteCustomHealthCheckResponse
});

app.UseAuthorization();

app.MapControllers();

app.Run();
