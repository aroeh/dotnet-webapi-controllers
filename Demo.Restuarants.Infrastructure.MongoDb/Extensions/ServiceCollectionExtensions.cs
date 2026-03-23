using Demo.Restuarants.Infrastructure.MongoDb.Documents;
using Demo.Restuarants.Infrastructure.MongoDb.Interfaces;
using Demo.Restuarants.Infrastructure.MongoDb.Options;
using Demo.Restuarants.Infrastructure.MongoDb.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Restuarants.API.MongoDb.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureRepos(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IMongoDbRepo<RestuarantDocument>, MongoDbRepo<RestuarantDocument>>();
        services.AddScoped<IRestuarantRepo, RestuarantRepo>();

        GetOptions(services, config);

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
}
