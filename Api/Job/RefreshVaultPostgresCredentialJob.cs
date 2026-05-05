using Api.Interface.Processing;
using Api.Model.Common;
using Quartz;
using QuartzScheduler.Interface;

namespace Api.Service.Job
{
    [DisallowConcurrentExecution]
    public class RefreshVaultPostgresCredentialJob(
        IApplicationSettingProcessingService iApplicationSettingProcessingService,
        IQuartzSchedulerBrokerService iQuartzSchedulerBrokerService,
        AppSettings appSettings,
        ILogger<RefreshVaultPostgresCredentialJob> iLogger
    ) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                AppSettings updatedAppSettings = await iApplicationSettingProcessingService.ProcessRetrieveVaultPostgresCredentialAsync(appSettings);

                iLogger.LogInformation("Postgres credential refreshed successfully");

                DateTimeOffset now = DateTimeOffset.UtcNow;
                DateTimeOffset runAt = now + (updatedAppSettings.PostgresSettings.ExpireAt - now) * 0.8;

                if (runAt <= DateTimeOffset.UtcNow)
                {
                    iLogger.LogWarning("Computed runAt {RunAt} is in the past. Skipping scheduling.", runAt);
                    return;
                }

                await iQuartzSchedulerBrokerService.ScheduleJobAsync<RefreshVaultPostgresCredentialJob>(runAt);

                iLogger.LogInformation("Scheduled RefreshVaultPostgresCredentialJob at {RunAt}", runAt);
            }
            catch (Exception ex)
            {
                iLogger.LogError(ex, "Failed to refresh Postgres credential");
                throw;
            }
        }
    }
}