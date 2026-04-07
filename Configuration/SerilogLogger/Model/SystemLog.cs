using System.Text.Json;
using Serilog.Events;
using Serilog.Formatting;

namespace SerilogLogger.Model
{
    public class SystemLog
    {
        public string OperatingSystem { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int ProcessId { get; set; } = 0;
        public string Application { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
    }

    public sealed class SystemLogFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            string renderedMessage = logEvent.RenderMessage();

            SystemLog systemLog = new()
            {
                OperatingSystem = Environment.OSVersion.ToString(),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                ProcessId = Environment.ProcessId,
                Application = (logEvent.Properties.ContainsKey("Application") ? logEvent.Properties["Application"].ToString() : AppDomain.CurrentDomain.FriendlyName).Trim('"'),
                Timestamp = logEvent.Timestamp.UtcDateTime,
                Level = logEvent.Level.ToString(),
                Message = renderedMessage,
                Exception = logEvent.Exception?.ToString()
            };

            output.WriteLine(JsonSerializer.Serialize(systemLog));
        }
    }
}