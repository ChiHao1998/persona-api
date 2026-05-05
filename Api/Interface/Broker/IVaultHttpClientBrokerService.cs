using Api.Model.Dto.Response;

namespace Api.Interface.Broker
{
    public interface IVaultHttpClientBrokerService
    {
        ValueTask<PostVaultAuthResponseDto> PostLoginAsync(CancellationToken cancellationToken = default);
        ValueTask<GetVaultRootSecretResponseDto> GetPersonaRootDataAsync(CancellationToken cancellationToken = default);
        ValueTask<VaultResponseWrapperDto<PostVaultDatabaseCredentialResponseDto>> PostPostgresCredentialAsync(string databaseEngineName, string databaseEngineRole, CancellationToken cancellationToken = default);
    }
}