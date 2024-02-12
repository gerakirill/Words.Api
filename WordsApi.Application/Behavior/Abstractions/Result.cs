namespace WordsApi.Application.Behavior.Abstractions;

public record Result<T>
{
    public Result(T data)
    {
        Data = data;
        OperationResult = OperationResult.Success;
    }

    public Result(OperationResult operationResult, IDictionary<string, string> errors)
    {
        OperationResult = operationResult;
        Errors = errors;
    }

    public bool IsSuccess => this.OperationResult == OperationResult.Success;

    public T? Data { get; } = default;

    public OperationResult OperationResult { get; }

    public IDictionary<string, string>? Errors { get; }
}