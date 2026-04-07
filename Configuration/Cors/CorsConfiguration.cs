using Cors.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection SetupCors(this IServiceCollection services, CorsPolicy[] corsPolicyList)
    {
        services.AddCors(options =>
        {
            foreach (CorsPolicy corsPolicy in corsPolicyList)
                options.AddPolicy(corsPolicy.Policy, policy =>
                    {
                        if (corsPolicy.AllowedOriginList is null)
                            policy.AllowAnyOrigin();
                        else
                            policy.WithOrigins(corsPolicy.AllowedOriginList);

                        if (corsPolicy.AllowedMethodList is null)
                            policy.AllowAnyMethod();
                        else
                            policy.WithMethods(corsPolicy.AllowedMethodList);

                        if (corsPolicy.AllowedHeaderList is null)
                            policy.AllowAnyHeader();
                        else
                            policy.WithHeaders(corsPolicy.AllowedHeaderList);

                        if (corsPolicy.ExposedHeaderList.Length > 1)
                            policy.WithExposedHeaders(corsPolicy.ExposedHeaderList);

                        if (corsPolicy.IsAllowCredentials)
                            policy.AllowCredentials();

                        policy.SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                    });
        });

        return services;
    }
}
