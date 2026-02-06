namespace Watchwi.Domain.Common;

public class Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public bool IsSuccess => Error == null;

    private Result(T? value, string? error)
    {
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value, null);
    public static Result<T> Failure(string error) => new(default, error);
}

public class Result
{
    public string? Error { get; }
    public bool IsSuccess => Error == null;

    private Result(string? error)
    {
        Error = error;
    }

    public static Result Success() => new(null);
    public static Result Failure(string error) => new(error);
}
