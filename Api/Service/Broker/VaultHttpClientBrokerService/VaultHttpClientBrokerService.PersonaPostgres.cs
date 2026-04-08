using System.Text.Json;
using Api.Interface.Broker;
using Api.Model.Dto.Response;
using Common.Interface;

namespace Api.Service.Broker.VaultHttpClientBrokerService
{
    public partial class VaultHttpClientBrokerService : IVaultHttpClientBrokerService, IScopedService
    {
        public async ValueTask<PostVaultDatabaseCredentialResponseDto> PostPostgresCredentialAsync(string databaseEngineName, string databaseEngineRole, CancellationToken cancellationToken = default)
        {
            string url = $"{appSettings.VaultHttpClient.BaseUrl}/v1/{databaseEngineName}/creds/{databaseEngineRole}";

            HttpResponseMessage response = await RequestAsync(
                HttpMethod.Get,
                url,
                bodyContent: null,
                cancellationToken);

            if ((int)response.StatusCode >= 400)
                throw new HttpRequestException(
                    $"Vault request failed: {(int)response.StatusCode} {response.ReasonPhrase} " + $"{await response.Content.ReadAsStringAsync(cancellationToken)}");

            return (await response.Content.ReadFromJsonAsync<VaultResponseWrapperDto<PostVaultDatabaseCredentialResponseDto>>(cancellationToken))?.Data ??
            throw new JsonException("Invalid response from Vault");
        }
    }
}
