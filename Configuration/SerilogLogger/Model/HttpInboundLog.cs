using System.Text.Json;
using Serilog.Events;
using Serilog.Formatting;
using SerilogLogger.Extension;

namespace SerilogLogger.Model
{
    public class HttpInboundLog
    {
        public string RequestId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string? QueryString { get; set; }
        public string ClientIp { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public int StatusCode { get; set; }
        public long ElapsedMs { get; set; }
        public string? Outcome { get; set; }
        public Dictionary<string, object>? RequestHeaders { get; set; }
        public Dictionary<string, object>? ResponseHeaders { get; set; }
    }

    public sealed class HttpInboundLogFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            var log = new HttpInboundLog
            {
                RequestId = logEvent.GetStringProperty("RequestId") ?? string.Empty,
                Timestamp = logEvent.Timestamp.UtcDateTime,
                Method = logEvent.GetStringProperty("Method") ?? string.Empty,
                Path = logEvent.GetStringProperty("Path") ?? string.Empty,
                QueryString = logEvent.GetStringProperty("QueryString"),
                ClientIp = logEvent.GetStringProperty("ClientIp") ?? string.Empty,
                UserId = logEvent.GetStringProperty("UserId"),
                Outcome = logEvent.GetStringProperty("Outcome"),
                StatusCode = logEvent.GetIntProperty("StatusCode") ?? 0,
                ElapsedMs = logEvent.GetLongProperty("ElapsedMs"),
                RequestHeaders = logEvent.GetObjectProperty<Dictionary<string, object>>("RequestHeaders"),
                ResponseHeaders = logEvent.GetObjectProperty<Dictionary<string, object>>("ResponseHeaders")
            };

            output.WriteLine(JsonSerializer.Serialize(log));
        }
    }
}