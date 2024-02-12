using System.Text.Json;
using WordsApi.Application.Behavior.Abstractions;
using WordsApi.Application.Behavior.MessageHandleStrategy;
using WordsApi.Application.Behavior.MessageHandleStrategy.CheckWord;
using WordsApi.Application.WordsChecker;
using WordsApi.Messaging;

namespace WordsApi.Creation;

public class MessageHandlerFactory(IWordsChecker checker)
{
    public Result<IMessageHandler> CreateHandler(Message message)
    {
        switch (message.Type)
        {
            case MessageType.CheckWord:
                try
                {
                    var request = JsonSerializer.Deserialize<CheckWordRequest>(message.Data.GetRawText());
                    return new Result<IMessageHandler>(new CheckWordMessageHandler(checker, request));
                }
                catch
                {
                    var deserializationErrors = new Dictionary<string, string>
                        { { "InvalidData", "Data format is different from message type" } };
                    return new Result<IMessageHandler>(OperationResult.BadCommand, deserializationErrors);
                }
            default:
                var invalidMessageErrors = new Dictionary<string, string>
                    { { "InvalidMessage", "Message type is not supported currently" } };
                return new Result<IMessageHandler>(OperationResult.BadCommand, invalidMessageErrors);
        }
    }
}