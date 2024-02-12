using WordsApi.Application.Behavior.Abstractions;

namespace WordsApi.Application.Behavior.MessageHandleStrategy;

public interface IMessageHandler
{
    Result<string> Handle();
}