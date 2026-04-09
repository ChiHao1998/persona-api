using System.Text.Json.Serialization;
using Cors.Model;
using Postgres.Model;

namespace Api.Model.Dto.Response
{
    public sealed class GetVaultRootSecretDataResponseDto
    {
        [JsonPropertyName("data")]
        public required GetVaultRootSecretResponseDto Data { get; set; }
    }
    public sealed class GetVaultRootSecretResponseDto
    {
        [JsonPropertyName("cors_policy_list")]
        public CorsPolicy[] CorsPolicyList { get; set; } = [];

        [JsonPropertyName("postgres_settings")]
        public required PostgresSettings PostgresSettings { get; set; }
    }
}