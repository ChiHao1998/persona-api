using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using SerilogLogger.Model;
namespace Configuration;

public static class SerilogLoggerConfiguration
{
    public static IHostBuilder SetupSerilog(this IHostBuilder hostBuilder, string applicationName)
    {
        string SYSTEM_LOG_FILE_NAME = $"{nameof(SystemLog)}_{Process.GetCurrentProcess().StartTime:yyyyMMdd_HHmmss}.json";
        string HTTP_OUTBOUND_LOG_FILE_NAME = $"{nameof(HttpOutboundLog)}_{DateTime.UtcNow:yyyyMMdd}.json";
        string HTTP_INBOUND_LOG_FILE_NAME = $"{nameof(HttpInboundLog)}_{DateTime.UtcNow:yyyyMMdd}.json";

        Log.CloseAndFlush();

        hostBuilder.UseSerilog((context, services, loggerConfig) =>
        {
            loggerConfig
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Extensions.Http", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.Extensions.Hosting", LogEventLevel.Fatal)
                .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Fatal)

                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("OS", RuntimeInformation.OSDescription)
                .Enrich.WithProperty("Application", applicationName)

                // 🔥 THIS is what bridges ILogger<T> → Serilog
                .ReadFrom.Services(services)

                // 🟢 SYSTEM LOGS
                .WriteTo.Async(a => a.Logger(lc => lc
                    // Exclude HTTP inbound outbound logs instead of excluding SourceContext
                    .Filter.ByExcluding(Matching.FromSource<HttpOutboundLog>())
                    .Filter.ByExcluding(Matching.FromSource<HttpInboundLog>())

                    .WriteTo.File(
                        new SystemLogFormatter(),
                        path: $"Log/{SYSTEM_LOG_FILE_NAME}",
                        rollingInterval: RollingInterval.Day
                    )

                    .WriteTo.Console(
                        outputTemplate:
                        "[{OS}]\t[HOST:{MachineName}]\t[USER:{EnvironmentUserName}]\t" +
                        "[PID:{ProcessId}]\t[{Application}]\t" +
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}]\t[{Level}]\n" +
                        " ↳ {Message:lj}{NewLine}{Exception}"
                    )
                ), bufferSize: 10_000, blockWhenFull: false)

                .WriteTo.Async(a => a.Logger(lc => lc
                    .Filter.ByIncludingOnly(Matching.FromSource<HttpInboundLog>())
                    .WriteTo.File(
                        new HttpInboundLogFormatter(),
                        path: $"Log/{HTTP_INBOUND_LOG_FILE_NAME}"
                    )
                ), bufferSize: 10_000, blockWhenFull: false)

                .WriteTo.Async(a => a.Logger(lc => lc
                    .Filter.ByIncludingOnly(Matching.FromSource<HttpOutboundLog>())
                    .WriteTo.File(
                        new HttpOutboundLogFormatter(),
                        path: $"Log/{HTTP_OUTBOUND_LOG_FILE_NAME}"
                    )
                ), bufferSize: 10_000, blockWhenFull: false);

        });

        return hostBuilder;
    }
}