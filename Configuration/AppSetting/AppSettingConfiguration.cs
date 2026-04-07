using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Configuration;

public static class AppSettingConfiguration
{
    public static WebApplicationBuilder InjectAppConfiguration<T>(this WebApplicationBuilder builder)
        where T : class
    {
        builder.Services.Configure<T>(builder.Configuration);

        return builder;
    }

    public static IServiceCollection InjectHostOptions(this IServiceCollection services)
    {
        services.Configure<HostOptions>(options =>
        {
            options.BackgroundServiceExceptionBehavior =
                BackgroundServiceExceptionBehavior.Ignore;
        });

        return services;
    }
    public static IServiceCollection SetMaxRequestBodySize(this IServiceCollection services, int maxRequestBodySizeInMb)
    {
        services.Configure<KestrelServerOptions>(opts =>
        {
            opts.Limits.MaxRequestBodySize = maxRequestBodySizeInMb * 1024 * 1024;
        });
        return services;
    }

    public static T RetrieveAppSetting<T>(this WebApplicationBuilder builder)
        where T : class
    {
        return builder.Configuration.Get<T>() ?? throw new InvalidOperationException("AppSetting not found");
    }

}
