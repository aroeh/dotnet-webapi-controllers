using Demo.Restuarants.API.Core.Interfaces;
using Demo.Restuarants.API.Core.Orchestrations;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Restuarants.API.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreOrchestrations(this IServiceCollection services)
    {
        services.AddTransient<IRestuarantOrchestration, RestuarantOrchestration>();

        return services;
    }
}