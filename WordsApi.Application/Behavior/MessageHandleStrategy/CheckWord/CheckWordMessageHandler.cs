using System.Text.Json;
using WordsApi.Application.Behavior.Abstractions;
using WordsApi.Application.WordsChecker;

namespace WordsApi.Application.Behavior.MessageHandleStrategy.CheckWord
{
    public class CheckWordMessageHandler(IWordsChecker checker, CheckWordRequest message) : IMessageHandler
    {
        public Result<string> Handle()
        {
            var responseData = new CheckWordResponseData(checker.CheckWord(message.Word));
            string data = JsonSerializer.Serialize(responseData);

            return new Result<string>(data);
        }
    }
}
