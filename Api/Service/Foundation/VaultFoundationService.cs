using Api.Interface.Broker;
using Api.Interface.Foundation;
using Api.Model.Common;
using Api.Model.Dto.Response;
using Common.Interface;
using Serilog;

namespace Api.Service.Foundation
{
    public class VaultFoundationService(
        IVaultHttpClientBrokerService iVaultHttpClientBrokerService,
        AppSettings appSettings
    ) : IVaultFoundationService, IScopedService
    {
        public async ValueTask AddLoginAsync(CancellationToken cancellationToken = default)
        {
            PostVaultAuthResponseDto postVaultAuthResponseDto = await iVaultHttpClientBrokerService.PostLoginAsync(cancellationToken);

            DateTimeOffset expiryDate = DateTimeOffset.UtcNow.AddSeconds(postVaultAuthResponseDto.LeaseDuration);

            appSettings.VaultHttpClient.ExpireAt = expiryDate;
            appSettings.VaultHttpClient.Token = postVaultAuthResponseDto.ClientToken;

            Log.Information($"Vault token retrieved. (Expired at {appSettings.VaultHttpClient.ExpireAt})");

            return;
        }

        public async ValueTask<AppSettings> RetrieveVaultRootSecretAsync(CancellationToken cancellationToken = default)
        {
            GetVaultRootSecretResponseDto getVaultRootSecretResponseDto = await iVaultHttpClientBrokerService.GetPersonaRootDataAsync(cancellationToken);

            appSettings.CorsPolicyList = getVaultRootSecretResponseDto.CorsPolicyList;
            appSettings.PostgresSettings = getVaultRootSecretResponseDto.PostgresSettings;

            Log.Information("Vault secret settings retrieved");

            return appSettings;
        }

        public async ValueTask<AppSettings> RetrieveVaultPostgresCredentialAsync(string databaseEngineName, string databaseEngineRole, CancellationToken cancellationToken = default)
        {
            VaultResponseWrapperDto<PostVaultDatabaseCredentialResponseDto> vaultResponseWrapperDto = await iVaultHttpClientBrokerService.PostPostgresCredentialAsync(databaseEngineName, databaseEngineRole, cancellationToken);

            appSettings.PostgresSettings.Username = vaultResponseWrapperDto.Data.Username;
            appSettings.PostgresSettings.Password = vaultResponseWrapperDto.Data.Password;
            appSettings.PostgresSettings.ExpireAt = DateTimeOffset.UtcNow.AddSeconds(vaultResponseWrapperDto.LeaseDuration);

            Log.Information("Vault postgres temporary credentials retrieved");

            return appSettings;
        }
    }
}