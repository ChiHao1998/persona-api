using Common.Interface;
using Cors.Model;
using Postgres.Model;
using QuartzScheduler.Model;

namespace Api.Model.Common
{
    public class AppSettings : ISingletonService
    {
        public VaultHttpClient VaultHttpClient { get; set; } = new();
        public CorsPolicy[] CorsPolicyList { get; set; } = [];
        public PostgresSettings PostgresSettings { get; set; } = new();
        public QuartzSettings QuartzSettings { get; set; } = new();

        public string GenerateQuartzConnectionString() => $"Host={PostgresSettings.Host};Port={PostgresSettings.Port};Database={PostgresSettings.DatabaseName};Username={QuartzSettings.Username};Password={QuartzSettings.Password};Pooling=false;";
    }
    public class VaultHttpClient
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTimeOffset ExpireAt { get; set; } = DateTimeOffset.UtcNow;
    }
}