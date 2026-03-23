using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Demo.Restuarants.API.SDK;

public static class SdkExtensions
{
    public static IServiceCollection AddRestuarantApi(this IServiceCollection services, IConfiguration config)
    {
        var options = GetOptions(services, config);

        RefitSettings refitSettings = new()
        {
            /*
             * Instructs Refit how to read query parameters for collection types
             * Multi specifies using multiple parameter instances
             * ex: name=value1&name=value2
             */
            CollectionFormat = CollectionFormat.Multi
        };

        services
            .AddRefitClient<IRestuarantApi>(refitSettings)
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(options.BaseUrl));

        return services;
    }

    private static RestuarantApiOptions GetOptions(IServiceCollection services, IConfiguration config)
    {
        var configSettings = config.GetRequiredSection(RestuarantApiOptions.ConfigKey);

        var options = configSettings.Get<RestuarantApiOptions>() ?? throw new Exception("Restuarant API configuration is missing");
        if (options is not null)
        {
            if (string.IsNullOrWhiteSpace(options.BaseUrl))
            {
                throw new Exception("BaseUrl is missing");
            }

            if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _))
            {
                throw new Exception("BaseUrl is not a properly formatted URL");
            }
        }

        return options!;
    }
}
