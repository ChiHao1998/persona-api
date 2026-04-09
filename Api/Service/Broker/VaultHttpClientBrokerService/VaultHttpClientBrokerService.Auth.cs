using System.Text.Json;
using Api.Interface.Broker;
using Api.Model.Dto.Response;
using Common.Interface;

namespace Api.Service.Broker.VaultHttpClientBrokerService
{
    public partial class VaultHttpClientBrokerService : IVaultHttpClientBrokerService, IScopedService
    {
        public async ValueTask<PostVaultAuthResponseDto> PostLoginAsync(CancellationToken cancellationToken = default)
        {
            string url = $"{appSettings.VaultHttpClient.BaseUrl}/v1/auth/userpass/login/{appSettings.VaultHttpClient.Username}";

            HttpResponseMessage response = await RequestAsync(HttpMethod.Post, url, bodyContent: new { password = appSettings.VaultHttpClient.Password }, cancellationToken);

            if ((int)response.StatusCode >= 400)
                throw new HttpRequestException($"Vault request failed: {(int)response.StatusCode} {response.ReasonPhrase} {await response.Content.ReadAsStringAsync(cancellationToken)}");

            PostVaultAuthResponseDto postVaultAuthResponseDto = (await response.Content.ReadFromJsonAsync<VaultResponseWrapperDto>(cancellationToken: cancellationToken))?.Auth ?? throw new JsonException("Invalid response from Vault");

            httpClient.DefaultRequestHeaders.Remove("X-Vault-Token");
            httpClient.DefaultRequestHeaders.Add("X-Vault-Token", postVaultAuthResponseDto.ClientToken);

            return postVaultAuthResponseDto;
        }
    }
}
