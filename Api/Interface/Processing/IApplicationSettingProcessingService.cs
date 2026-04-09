using Api.Model.Common;

namespace Api.Interface.Processing
{
    public interface IApplicationSettingProcessingService
    {
        ValueTask<AppSettings> ProcessRetrieveVaultAppSettingsAsync(CancellationToken cancellationToken = default);
        ValueTask<AppSettings> ProcessRetrieveVaultPostgresCredentialAsync(AppSettings appSettings, CancellationToken cancellationToken = default);
    }
}