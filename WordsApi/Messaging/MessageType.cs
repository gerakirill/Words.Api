using WordsApi.Infrastructure.Auth;

namespace WordsApi.Messaging;

public enum MessageType : byte
{
    [Authorize(Scope = "api.read")]
    CheckWord = 1
}