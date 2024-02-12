using System.Text.Json;
using System.Text.Json.Serialization;
using WordsApi.Messaging.Converters;

namespace WordsApi.Messaging;

public class Message
{
    [JsonConverter(typeof(MessageTypeConverter))]
    public required MessageType Type { get; set; }

    public required JsonElement Data { get; set; }
}