using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Postgres.Model;

namespace Configuration;

public static class PostgresConfiguration
{
    public static IServiceCollection SetupPostgresDatabaseContext<T>(this IServiceCollection services, PostgresSettings? postgresSettings = null)
    where T : DbContext
    {
        services.AddDbContextFactory<T>((sp, options) =>
        {
            postgresSettings ??= sp.GetRequiredService<IOptionsMonitor<PostgresSettings>>().CurrentValue;

            options.UseNpgsql(postgresSettings.GenerateConnectionString(), npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);

                npgsqlOptions.CommandTimeout(30);

                npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            options.EnableDetailedErrors();
        });

        return services;
    }

    public static async Task CheckAndApplyMigrationsAsync<T>(this IServiceProvider services, CancellationToken cancellationToken = default)
    where T : DbContext
    {
        using IServiceScope scope = services.CreateScope();
        ILogger logger = scope.ServiceProvider.GetRequiredService<ILogger<T>>();
        T db = scope.ServiceProvider.GetRequiredService<T>();

        List<string> pendingMigrationList = [.. await db.Database
            .GetPendingMigrationsAsync(cancellationToken)];

        if (pendingMigrationList.Count == 0)
        {
            logger.LogInformation("Database schema is up-to-date.");
            return;
        }

        logger.LogWarning($"Database has {pendingMigrationList.Count} pending migrations: {string.Join(", ", pendingMigrationList)}");

        // PostgreSQL advisory lock (one runner only)
        await db.Database.ExecuteSqlRawAsync(
            "SELECT pg_advisory_lock(123456789);",
            cancellationToken);

        try
        {
            await db.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Migrations applied successfully.");
        }
        finally
        {
            await db.Database.ExecuteSqlRawAsync(
                "SELECT pg_advisory_unlock(123456789);",
                cancellationToken);
        }
    }
}