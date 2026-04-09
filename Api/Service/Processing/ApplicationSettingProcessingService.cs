using Api.Interface.Foundation;
using Api.Interface.Processing;
using Api.Model.Common;
using Common.Interface;

namespace Api.Service.Processing
{
    public class ApplicationSettingProcessingService
    (
        IVaultFoundationService iVaultFoundationService
    )
    : IApplicationSettingProcessingService, IScopedService
    {
        public async ValueTask<AppSettings> ProcessRetrieveVaultAppSettingsAsync(CancellationToken cancellationToken = default)
        {
            await iVaultFoundationService.AddLoginAsync(cancellationToken);

            return await iVaultFoundationService.RetrieveVaultRootSecretAsync(cancellationToken);
        }

        public async ValueTask<AppSettings> ProcessRetrieveVaultPostgresCredentialAsync(AppSettings appSettings, CancellationToken cancellationToken = default)
        {
            await iVaultFoundationService.RetrieveVaultPostgresCredentialAsync(appSettings.PostgresSettings.VaultDatabaseEngineName, appSettings.PostgresSettings.VaultDatabaseEngineRole, cancellationToken);

            return appSettings;
        }
    }
}