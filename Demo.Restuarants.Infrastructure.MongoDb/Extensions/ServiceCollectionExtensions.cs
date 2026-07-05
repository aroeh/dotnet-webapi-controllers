using Demo.Restuarants.Infrastructure.MongoDb.Documents;
using Demo.Restuarants.Infrastructure.MongoDb.Interfaces;
using Demo.Restuarants.Infrastructure.MongoDb.Options;
using Demo.Restuarants.Infrastructure.MongoDb.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Demo.Restuarants.Infrastructure.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureRepos(this IServiceCollection services, IConfiguration config)
    {
        GetOptions(services, config);

        services.AddMongoDbFactory<RestuarantDocument>("restuarants");
        services.AddScoped<IRestuarantRepo, RestuarantRepo>();

        return services;
    }

    private static void GetOptions(IServiceCollection services, IConfiguration config)
    {
        var configSettings = config.GetRequiredSection(MongoDbOptions.ConfigKey);

        var options = configSettings.Get<MongoDbOptions>();

        if(options is not null)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                throw new Exception("MongoDb Connection string is missing");
            }

            if (string.IsNullOrWhiteSpace(options.DatabaseName))
            {
                throw new Exception("MongoDb database name is missing");
            }
        }

        services.Configure<MongoDbOptions>(configSettings);
    }

    private static void AddMongoDbFactory<T>(this IServiceCollection services, string collectionName) where T : class
    {
        services.AddScoped<IMongoDbRepo<T>, MongoDbRepo<T>>(sp =>
        {
            ILogger<MongoDbRepo<T>> logger = sp.GetRequiredService<ILogger<MongoDbRepo<T>>>();
            IOptions<MongoDbOptions> options = sp.GetRequiredService<IOptions<MongoDbOptions>>();
            return new MongoDbRepo<T>(logger, options, collectionName);
        });
    }
}
