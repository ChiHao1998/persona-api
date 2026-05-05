using Api.Model.Common;
using Api.Service.Job;
using QuartzScheduler.Interface;

namespace Api.Service.Background
{
    public class RefreshVaultPostgresCredentialBackgroundService(
        IServiceProvider serviceProvider,
        AppSettings appSettings,
        ILogger<RefreshVaultPostgresCredentialBackgroundService> iLogger
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            IQuartzSchedulerBrokerService schedulerBroker = scope.ServiceProvider.GetRequiredService<IQuartzSchedulerBrokerService>();

            try
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;
                DateTimeOffset runAt = now + (appSettings.PostgresSettings.ExpireAt - now) * 0.8;

                if (runAt <= DateTimeOffset.UtcNow)
                {
                    iLogger.LogWarning("Computed runAt {RunAt} is in the past. Skipping scheduling.", runAt);
                    return;
                }

                await schedulerBroker.ScheduleJobAsync<RefreshVaultPostgresCredentialJob>(runAt, cancellationToken);

                iLogger.LogInformation("Scheduled RefreshVaultPostgresCredentialJob at {RunAt}", runAt);
            }
            catch (Exception ex)
            {
                iLogger.LogError(ex, "Failed to schedule Vault Postgres credential refresh job on startup");
            }
        }
    }
}