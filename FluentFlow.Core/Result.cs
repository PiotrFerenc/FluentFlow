namespace FluentFlow.Core;

public class Result<T>
{
    public T? Value { get; private set; }
    public string Error { get; }

    private Result(T value)
    {
        Value = value;
        Error = string.Empty;
    }

    private Result(string error)
    {
        Error = error;
    }

    private static Result<T> Success(T value) => new Result<T>(value);
    public static Result<T> Failed(string message) => new Result<T>(message);
    
    public bool IsSuccess => string.IsNullOrEmpty(Error);
    public bool IsFailed => !IsSuccess;
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(string message) => Failed(message);
}