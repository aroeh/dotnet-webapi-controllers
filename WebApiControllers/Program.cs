using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using WebApiControllers.DomainLogic.Orchestrations;
using WebApiControllers.Health;
using WebApiControllers.Infrastructure.Extensions;
using WebApiControllers.Infrastructure.Repos;
using WebApiControllers.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add and include additional app settings files
// by default
// builder.Configuration
//     .AddJsonFile("appsettings.json")
//     .AddJsonFile("appsettings.Development.json");

// Add services to the container.

// register a custom exception handler middleware and problem details usage in the pipeline
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// configure authoriation and authentication for the API with Microsoft EntraID
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// add health checks to the app
// this is currently using a custom mongodb health check
builder.Services.AddHealthChecks()
    .AddCheck<CustomMongoDbHealthCheck>("Database");

// setup API output cache policy to use on specified controllers
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder =>
        builder.Expire(TimeSpan.FromSeconds(60)));
});

builder.Services.AddControllers();

// setup api versioning using url path versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Restuarant API V2",
            Version = "v2",
            Description = "Expanded and full version of the resutarant api"
        };
        //document.Paths = new()
        //{
        //    a
        //};
        return Task.CompletedTask;
    });
});


// Add classes and interfaces for dependency injection
// transient is being used here so new instances of classes are insantiated when needed in the request pipeline
builder.Services.AddTransient<IDatabaseWrapper, DatabaseWrapper>();
builder.Services.AddTransient<IRestuarantOrchestration, RestuarantOrchestration>();
builder.Services.AddTransient<IRestuarantRepo, RestuarantRepo>();

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
    //ApiVersionSetBuilder versionBuilder = new("restuarant");
    //versionBuilder.HasApiVersion(new Asp.Versioning.ApiVersion(2));
    //versionBuilder.HasDeprecatedApiVersion(new Asp.Versioning.ApiVersion(1));
    //versionBuilder.ReportApiVersions();
    app.MapOpenApi();
        //.WithApiVersionSet(new ApiVersionSet(versionBuilder, "restuarant"))
        //.ReportApiVersions();
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

app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();
