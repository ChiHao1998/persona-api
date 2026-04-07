using Serilog.Events;

namespace SerilogLogger.Extension
{
    public static class LogEventExtension
    {
        public static string? GetStringProperty(this LogEvent logEvent, string name)
        {
            if (!logEvent.Properties.TryGetValue(name, out var value))
                return null;

            if (value is ScalarValue scalar)
                return scalar.Value as string;

            return null;
        }

        public static int? GetIntProperty(this LogEvent logEvent, string name)
        {
            return logEvent.Properties.TryGetValue(name, out LogEventPropertyValue? value)
                && int.TryParse(value.ToString(), out var result)
                ? result
                : null;
        }

        public static long GetLongProperty(this LogEvent logEvent, string name)
        {
            return logEvent.Properties.TryGetValue(name, out LogEventPropertyValue? value)
                && long.TryParse(value.ToString(), out var result)
                ? result
                : 0;
        }

        public static T? GetObjectProperty<T>(this LogEvent logEvent, string name)
        {
            if (!logEvent.Properties.TryGetValue(name, out LogEventPropertyValue? value))
                return default;

            object? obj = ConvertLogEventValue(value);

            return obj is T typed ? typed : default;
        }

        private static object? ConvertLogEventValue(LogEventPropertyValue value)
        {
            return value switch
            {
                ScalarValue sv => sv.Value,

                SequenceValue seq => seq.Elements
                    .Select(ConvertLogEventValue)
                    .ToArray(),

                StructureValue str => str.Properties
                    .ToDictionary(
                        p => p.Name,
                        p => ConvertLogEventValue(p.Value)
                    ),

                DictionaryValue dict => dict.Elements
                    .ToDictionary(
                        e => e.Key.Value?.ToString() ?? string.Empty,
                        e => ConvertLogEventValue(e.Value)
                    ),

                _ => null
            };
        }
    }
}