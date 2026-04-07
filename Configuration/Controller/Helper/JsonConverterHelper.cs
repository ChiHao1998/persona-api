using System.Text.Json;
using System.Text.Json.Serialization;

namespace Controller.Helper
{
    public sealed class JsonConverterHelper : JsonConverter<string?>
    {
        public override string? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException($"Unexpected token {reader.TokenType}");

            var value = reader.GetString()?.Trim();

            return string.IsNullOrEmpty(value) ? null : value;
        }

        public override void Write(
            Utf8JsonWriter writer,
            string? value,
            JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Trim());
        }
    }
}