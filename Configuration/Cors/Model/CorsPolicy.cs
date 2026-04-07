using System.Text.Json.Serialization;

namespace Cors.Model
{
    public sealed class CorsPolicy
    {
        [JsonPropertyName("policy")]
        public required string Policy { get; set; }

        [JsonPropertyName("is_allow_credentials")]
        public bool IsAllowCredentials { get; set; } = true;

        [JsonPropertyName("allowed_origin_list")]
        public string[]? AllowedOriginList { get; set; }

        [JsonPropertyName("allowed_method_list")]
        public string[]? AllowedMethodList { get; set; }

        [JsonPropertyName("allowed_header_list")]
        public string[]? AllowedHeaderList { get; set; }

        [JsonPropertyName("exposed_header_list")]
        public string[] ExposedHeaderList { get; set; } = [];
    }
}