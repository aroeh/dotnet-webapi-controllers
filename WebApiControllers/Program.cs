using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using WebApiControllers.DataAccess;
using WebApiControllers.Health;
using WebApiControllers.Middleware;
using WebApiControllers.Repos;

namespace WebApiControllers
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
            // this is currently using a custom mongodb health check.
            // we could use a built in one from libraries - comment out the custom health check and uncomment the line .AddMongoDb
            builder.Services.AddHealthChecks()
                //.AddMongoDb("connection string goes here");
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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add classes and interfaces for dependency injection
            // transient is being used here so new instances of classes are insantiated when needed in the request pipeline
            builder.Services.AddTransient<IMongoService, MongoService>();
            builder.Services.AddTransient<IRestuarantRepo, RestuarantRepo>();
            builder.Services.AddTransient<IRestuarantData, RestuarantData>();

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
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // use the global exception handler
            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            // using AspNetCore.HealthChecks.UI.Client to setup the response
            app.MapHealthChecks("/_health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseOutputCache();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
