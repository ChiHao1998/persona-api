using System.Text.Json.Serialization;

namespace Api.Model.Dto.Response
{
    public sealed class VaultResponseWrapperDto<T>
    {
        [JsonPropertyName("request_id")]
        public string RequestId { get; init; } = string.Empty;

        [JsonPropertyName("lease_id")]
        public string LeaseId { get; init; } = string.Empty;

        [JsonPropertyName("renewable")]
        public bool Renewable { get; init; }

        [JsonPropertyName("lease_duration")]
        public int LeaseDuration { get; init; }

        [JsonPropertyName("data")]
        public required T Data { get; init; }

        [JsonPropertyName("wrap_info")]
        public object? WrapInfo { get; init; }

        [JsonPropertyName("warnings")]
        public object? Warnings { get; init; }

        [JsonPropertyName("auth")]
        public PostVaultAuthResponseDto? Auth { get; init; }

        [JsonPropertyName("mount_type")]
        public string MountType { get; init; } = string.Empty;
    }

    public sealed class VaultResponseWrapperDto
    {
        [JsonPropertyName("request_id")]
        public string RequestId { get; init; } = string.Empty;

        [JsonPropertyName("lease_id")]
        public string LeaseId { get; init; } = string.Empty;

        [JsonPropertyName("renewable")]
        public bool Renewable { get; init; }

        [JsonPropertyName("lease_duration")]
        public int LeaseDuration { get; init; }

        [JsonPropertyName("data")]
        public object? Data { get; init; }

        [JsonPropertyName("wrap_info")]
        public object? WrapInfo { get; init; }

        [JsonPropertyName("warnings")]
        public object? Warnings { get; init; }

        [JsonPropertyName("auth")]
        public PostVaultAuthResponseDto? Auth { get; init; }

        [JsonPropertyName("mount_type")]
        public string MountType { get; init; } = string.Empty;
    }
}