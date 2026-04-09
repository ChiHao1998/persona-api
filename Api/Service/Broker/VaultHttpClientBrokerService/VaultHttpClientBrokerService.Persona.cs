using System.Text.Json;
using Api.Interface.Broker;
using Api.Model.Dto.Response;
using Common.Interface;

namespace Api.Service.Broker.VaultHttpClientBrokerService
{
    public partial class VaultHttpClientBrokerService : IVaultHttpClientBrokerService, IScopedService
    {
        public async ValueTask<GetVaultRootSecretResponseDto> GetPersonaRootDataAsync(CancellationToken cancellationToken = default)
        {
            string url = $"{appSettings.VaultHttpClient.BaseUrl}/v1/{appSettings.VaultHttpClient.Username}/data/root";

            HttpResponseMessage response = await RequestAsync(HttpMethod.Get, url, bodyContent: null, cancellationToken);

            if ((int)response.StatusCode >= 400)
                throw new HttpRequestException($"Vault request failed: {(int)response.StatusCode} {response.ReasonPhrase} {await response.Content.ReadAsStringAsync(cancellationToken)}");

            GetVaultRootSecretDataResponseDto getVaultRootSecretDataResponseDto = (await response.Content.ReadFromJsonAsync<VaultResponseWrapperDto<GetVaultRootSecretDataResponseDto>>(cancellationToken: cancellationToken))?.Data ?? throw new JsonException("Invalid response from Vault");

            return getVaultRootSecretDataResponseDto.Data;
        }
    }
}
