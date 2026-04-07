using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Configuration;

public static class QuartzSchedulerConfiguration
{
    public static IServiceCollection SetupQuartzScheduler(this IServiceCollection services, string connectionstrings)
    {
        services.AddQuartz(options =>
        {
            options.UsePersistentStore(s =>
            {
                s.UsePostgres(options =>
                {
                    options.TablePrefix = "quartz_";
                }, connectionstrings);
                s.UseClustering();
            });

            options.MisfireThreshold = TimeSpan.FromSeconds(30);
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}
