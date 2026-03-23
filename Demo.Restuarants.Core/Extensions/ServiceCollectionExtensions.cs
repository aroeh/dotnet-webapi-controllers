using Demo.Restuarants.Core.Interfaces;
using Demo.Restuarants.Core.Orchestrations;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Restuarants.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreOrchestrations(this IServiceCollection services)
    {
        services.AddTransient<IRestuarantOrchestration, RestuarantOrchestration>();

        return services;
    }
}