using Api.Interface.Processing;
using Api.Model.Common;
using Serilog;

namespace Api.Service.Startup
{
    public static class VaultStartupService
    {
        public async static ValueTask<AppSettings> RetrieveVaultAppSettingsAsync(this IServiceCollection iServiceCollection)
        {
            ServiceProvider serviceProvider = iServiceCollection.BuildServiceProvider();

            using IServiceScope scope = serviceProvider.CreateScope();

            IApplicationSettingProcessingService iApplicationSettingProcessingService = scope.ServiceProvider.GetRequiredService<IApplicationSettingProcessingService>();

            AppSettings appSettings = new();

            try
            {
                appSettings = await iApplicationSettingProcessingService.ProcessRetrieveVaultAppSettingsAsync(cancellationToken: default);

                appSettings = await iApplicationSettingProcessingService.ProcessRetrieveVaultPostgresCredentialAsync(appSettings, cancellationToken: default);

            }
            catch (Exception exception)
            {
                Log.Error("Error occurred while retrieving vault app settings: {ExceptionMessage}", exception.Message);
            }

            iServiceCollection.AddSingleton(appSettings);

            return appSettings;
        }
    }
}