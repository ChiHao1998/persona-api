using System.Text.Json.Serialization;

namespace Api.Model.Dto.Response
{
    public sealed class PostVaultAuthResponseDto
    {
        [JsonPropertyName("client_token")]
        public string ClientToken { get; init; } = string.Empty;

        [JsonPropertyName("accessor")]
        public string Accessor { get; init; } = string.Empty;

        [JsonPropertyName("policies")]
        public string[] Policies { get; init; } = [];

        [JsonPropertyName("token_policies")]
        public string[] TokenPolicies { get; init; } = [];

        [JsonPropertyName("metadata")]
        public VaultAuthMetadata Metadata { get; init; } = new();

        [JsonPropertyName("lease_duration")]
        public int LeaseDuration { get; init; }

        [JsonPropertyName("renewable")]
        public bool Renewable { get; init; }

        [JsonPropertyName("entity_id")]
        public string EntityId { get; init; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; init; } = string.Empty;

        [JsonPropertyName("orphan")]
        public bool Orphan { get; init; }

        [JsonPropertyName("mfa_requirement")]
        public object? MfaRequirement { get; init; }

        [JsonPropertyName("num_uses")]
        public int NumUses { get; init; }
    }

    public sealed class VaultAuthMetadata
    {
        [JsonPropertyName("username")]
        public string Username { get; init; } = string.Empty;
    }
}