namespace Online_Learning_Platform_Ass1.Service.Results;

public class ServiceResult<T>
{
    public bool Success { get; init; }
    public T? Data { get; set; }
    public string? Message { get; init; }
    public List<string> Errors { get; set; } = [];

    public static ServiceResult<T> SuccessResultAsync(T data, string? message = null) => new()
    {
        Success = true,
        Data = data,
        Message = message
    };

    public static ServiceResult<T> FailureResultAsync(string message) => new()
    {
        Success = false,
        Message = message
    };

    public static ServiceResult<T> FailureResultAsync(List<string> errors) => new()
    {
        Success = false,
        Errors = errors,
        Message = "Operation failed with multiple errors"
    };
}
