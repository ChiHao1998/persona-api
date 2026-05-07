using System.Text.Json.Serialization;

namespace QuartzScheduler.Model
{
    public sealed class QuartzSettings
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
