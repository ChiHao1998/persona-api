using System.Text.Json.Serialization;

namespace Api.Model.Dto.Response
{
    public sealed class GetVaultSecretResponseDto<T>
    where T : class
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}