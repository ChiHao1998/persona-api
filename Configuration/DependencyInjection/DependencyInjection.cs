using System.Reflection;
using Common.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AutoAddScopedServiceByIScopedService(this IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime();
        });

        return services;
    }
}
