namespace Core.Utils;

public class Result<T>(bool isSuccess, T data, string? errorMessage)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public string? ErrorMessage { get; set; } = errorMessage;
    public T Data { get; set; } = data;
    
    public static Result<T> Success(T data) => new (true, data, null);
    public static Result<T?> Failure(string error) =>
        new (false, default, error);
}

public class Result(bool isSuccess, string? errorMessage)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public string? ErrorMessage { get; set; } = errorMessage;
    
    public static Result Success() => new (true, null);
    public static Result Failure(string error) =>
        new (false, error);
}