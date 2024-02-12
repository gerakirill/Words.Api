namespace WordsApi.Application.Behavior.Abstractions;

public enum OperationResult : byte
{
    Success = 1,
    BadCommand,
    NotFound
}