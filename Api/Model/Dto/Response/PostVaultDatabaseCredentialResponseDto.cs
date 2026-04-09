using System.Text.Json.Serialization;

namespace Api.Model.Dto.Response
{
    public sealed class PostVaultDatabaseCredentialResponseDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = default!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = default!;

        [JsonPropertyName("lease_duration")]
        public int LeaseDuration { get; set; }
    }
}