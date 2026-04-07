using System.Diagnostics;
using System.Text.Json;
using Serilog.Events;
using Serilog.Formatting;
using SerilogLogger.Extension;

namespace SerilogLogger.Model
{
    public class HttpOutboundLog
    {
        public string OperatingSystem { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
        public string Application { get; set; } = string.Empty;
        public string? TraceId { get; set; }
        public string RequestId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int? StatusCode { get; set; }
        public long ElapsedMs { get; set; }
        public Dictionary<string, object>? RequestHeaders { get; set; }
        public Dictionary<string, object>? ResponseHeaders { get; set; }
    }

    public sealed class HttpOutboundLogFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            var log = new HttpOutboundLog
            {
                OperatingSystem = Environment.OSVersion.ToString(),
                MachineName = Environment.MachineName,
                Application = (logEvent.Properties.ContainsKey("Application") ? logEvent.Properties["Application"].ToString() : AppDomain.CurrentDomain.FriendlyName).Trim('"'),
                TraceId = Activity.Current?.TraceId.ToString(),
                RequestId = logEvent.GetStringProperty("RequestId") ?? string.Empty,

                Timestamp = logEvent.Timestamp.UtcDateTime,

                Method = logEvent.GetStringProperty("Method") ?? string.Empty,
                Host = logEvent.GetStringProperty("Host") ?? string.Empty,
                Path = logEvent.GetStringProperty("Path") ?? string.Empty,

                StatusCode = logEvent.GetIntProperty("StatusCode"),
                ElapsedMs = logEvent.GetLongProperty("ElapsedMs"),

                RequestHeaders = logEvent.GetObjectProperty<Dictionary<string, object>>("RequestHeaders"),
                ResponseHeaders = logEvent.GetObjectProperty<Dictionary<string, object>>("ResponseHeaders")
            };

            output.WriteLine(JsonSerializer.Serialize(log));
        }
    }

}