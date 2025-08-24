namespace WebServer.Models;

public sealed record Error(string Code, string? Description);

public class Result<T>
{
    private Result(bool isSuccess, Error? error = null, T? value = default)
    {
        IsSuccess = isSuccess;
        Error = error;
        Value = value;
    }

    public T? Value { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    public static Result<T> Success(T value) => new(true, null, value);

    public static Result<T> Failure(Error error) => new(false, error, default);
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Failure(Error error) => new(false, error);

    public static Result Success() => new(true);
}
