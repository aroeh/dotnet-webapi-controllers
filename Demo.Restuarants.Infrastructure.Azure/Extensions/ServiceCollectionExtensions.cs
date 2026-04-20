using Demo.Restuarants.Infrastructure.Azure.Models;
using Demo.Restuarants.Infrastructure.Azure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;


namespace Demo.Restuarants.Infrastructure.Azure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureAppConfigSettings(this ConfigurationManager config, IServiceCollection services, string environmentName)
    {
        AppConfigOptions appConfigOptions = GetOptions(config);

        config.AddAzureAppConfiguration(options =>
        {
            if (!string.IsNullOrWhiteSpace(appConfigOptions.ConnectionString))
            {
                options.Connect(appConfigOptions.ConnectionString);
            }

            if (!string.IsNullOrWhiteSpace(appConfigOptions.Endpoint))
            {
                options.Connect(appConfigOptions.Endpoint);
            }

            // enables getting app config key values without labels and labels filtered by the environment name
            options.Select(KeyFilter.Any, LabelFilter.Null);
            options.Select(KeyFilter.Any, environmentName);
        });

        services.Configure<Settings>(config.GetSection("RestuarantApi:Settings"));

        return services;
    }

    private static AppConfigOptions GetOptions(IConfiguration config)
    {
        var configSettings = config.GetRequiredSection(AppConfigOptions.ConfigKey);

        var options = configSettings.Get<AppConfigOptions>();

        if (options is not null)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionString) && string.IsNullOrWhiteSpace(options.Endpoint))
            {
                throw new Exception("The App Config `ConnectionString` and `Endpoint` are both missing.  One or other is required.");
            }

            if (!string.IsNullOrWhiteSpace(options.ConnectionString) && !string.IsNullOrWhiteSpace(options.Endpoint))
            {
                throw new Exception("The App Config `ConnectionString` and `Endpoint` are both set.  Only one must be configured.");
            }
        }

        return options!;
    }
}
