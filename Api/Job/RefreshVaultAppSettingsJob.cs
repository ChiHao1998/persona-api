using Api.Interface.Processing;
using Api.Model.Common;
using Quartz;
using QuartzScheduler.Interface;

namespace Api.Service.Job
{
    [DisallowConcurrentExecution]
    public class RefreshVaultAppSettingsJob(
        IApplicationSettingProcessingService iApplicationSettingProcessingService,
        IQuartzSchedulerBrokerService iQuartzSchedulerBrokerService,
        ILogger<RefreshVaultAppSettingsJob> iLogger
    ) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                AppSettings updatedAppSettings = await iApplicationSettingProcessingService.ProcessRetrieveVaultAppSettingsAsync();

                iLogger.LogInformation("Vault settings refreshed successfully");

                DateTimeOffset now = DateTimeOffset.UtcNow;
                DateTimeOffset runAt = now + (updatedAppSettings.VaultHttpClient.ExpireAt - now) * 0.8;

                if (runAt <= DateTimeOffset.UtcNow)
                {
                    iLogger.LogWarning("Computed runAt {RunAt} is in the past. Skipping scheduling.", runAt);
                    return;
                }

                await iQuartzSchedulerBrokerService.ScheduleJobAsync<RefreshVaultAppSettingsJob>(runAt);

                iLogger.LogInformation("Scheduled RefreshVaultAppSettingsJob at {RunAt}", runAt);
            }
            catch (Exception ex)
            {
                iLogger.LogError(ex, "Failed to refresh Vault settings");
                throw;
            }
        }
    }
}