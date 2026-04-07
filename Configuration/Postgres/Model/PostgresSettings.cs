using System.Text.Json.Serialization;

namespace Postgres.Model
{
    public sealed class PostgresSettings
    {
        [JsonPropertyName("host")]
        public string Host { get; set; } = string.Empty;

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("database_name")]
        public string DatabaseName { get; set; } = string.Empty;

        [JsonPropertyName("vault_database_engine_name")]
        public string VaultDatabaseEngineName { get; set; } = string.Empty;

        [JsonPropertyName("vault_database_engine_role")]
        public string VaultDatabaseEngineRole { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTimeOffset? ExpireAt { get; set; } = null;

        [JsonIgnore]
        public string Username { get; set; } = string.Empty;

        [JsonIgnore]
        public string Password { get; set; } = string.Empty;

        public string GenerateConnectionString() => $"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password};Pooling=false;";
    }
}