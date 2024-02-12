using System.Text.Json;
using System.Text.Json.Serialization;

namespace WordsApi.Messaging.Converters
{
    public class MessageTypeConverter : JsonConverter<MessageType>
    {
        public override MessageType Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            reader.GetString()?.ToLowerInvariant() switch
        {
            "checkword" => MessageType.CheckWord,
            _ => throw new ArgumentOutOfRangeException()
        };

        public override void Write(
            Utf8JsonWriter writer,
            MessageType messageType,
            JsonSerializerOptions options) =>
            writer.WriteStringValue(messageType.ToString().ToLowerInvariant());
    }
}
