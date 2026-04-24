using Common.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

    public static IServiceCollection AutoAddSingletonServiceByISingletonService(this IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo<ISingletonService>())
            .AsSelfWithInterfaces()
            .WithSingletonLifetime();
        });

        List<ServiceDescriptor> descriptorList = [.. services.Where(s => typeof(ISingletonService).IsAssignableFrom(s.ServiceType))];

        foreach (ServiceDescriptor descriptor in descriptorList)
        {
            services.Replace(ServiceDescriptor.Singleton(descriptor.ServiceType, serviceProvider =>
            {
                IConfiguration config = serviceProvider.GetRequiredService<IConfiguration>();

                object instance = Activator.CreateInstance(descriptor.ServiceType)!;

                config.Bind(instance);

                return instance;
            }));
        }

        return services;
    }
}
