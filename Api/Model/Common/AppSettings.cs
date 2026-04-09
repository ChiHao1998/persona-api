using Cors.Model;
using Postgres.Model;

namespace Api.Model.Common
{
    public class AppSettings
    {
        public VaultHttpClient VaultHttpClient { get; set; } = new();
        public CorsPolicy[] CorsPolicyList { get; set; } = [];
        public PostgresSettings PostgresSettings { get; set; } = new();
    }
    public class VaultHttpClient
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTimeOffset? ExpireAt { get; set; } = null;
    }
}