using Microsoft.Extensions.DependencyInjection;

namespace DynamicUpdateComponent.Backend;

public static class DependencyInjectionRegistrations
{
    public static IServiceCollection AddBackendServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IComponent, Component1>()
            .AddSingleton<IComponent, Component2>()
            .AddSingleton<IBackyBackend, BackyBackend>();

        return services;
    }
}