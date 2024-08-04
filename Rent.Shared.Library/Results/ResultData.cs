namespace Rent.Shared.Library.Results;

public class Result<T>(bool isSuccess, List<Error> errors)
{
    public T? Data { get; private set; }
    public bool IsSuccess { get; protected set; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public List<Error> Errors { get; protected set; } = errors;
    
    public Result(bool isSuccess, T? data, List<Error> errors) : this(isSuccess, errors)
    {
        Data = data;
    }
    
    public static implicit operator Result<T>(Result resultData)
    {
        return new Result<T>(resultData.IsSuccess, resultData.Errors.ToList());
    }
}