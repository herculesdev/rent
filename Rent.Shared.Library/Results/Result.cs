using FluentValidation.Results;

namespace Rent.Shared.Library.Results;

public class Result(bool isSuccess, List<Error> errors)
{
    public bool IsSuccess { get; protected set; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public List<Error> Errors { get; protected set; } = errors;
    
    public static Result Success() => new Result(isSuccess: true, []);
    public static Result Failure() => new Result(isSuccess: false, []);
    public static Result Failure(string message) => new Result(isSuccess: false, [new Error(message)]);
    public static Result Failure(string key, string message) => new Result(isSuccess: false, [new Error(key, message)]);

    public static Result Failure(List<ValidationFailure> errors) => new(isSuccess: false,
        errors.Select(x => new Error(x.PropertyName, x.ErrorMessage)).ToList());

    /**
     * Result T
     */
    public static Result<T> Success<T>(T data) => new Result<T>(isSuccess: true, data, []);
}