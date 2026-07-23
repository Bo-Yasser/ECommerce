using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error = null)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("Success Result cannot has an Error");
        if(!isSuccess && error is null)
            throw new InvalidOperationException("Failure Result Must has an Error");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true);
    public static Result Failure(Error error) => new(false, error);

}


public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;
    private Result(TValue? value, bool isSuccess, Error? error = null) : base(isSuccess, error)
    {
        _value = value;
    }
    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access the value of failed result.");

    public static Result<TValue> Success(TValue value) => new(value, true);
    public new static Result<TValue> Failure(Error error) => new(default, false, error);

    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {

        return IsSuccess ? onSuccess(_value!) : onFailure(Error!);
    }

    public static implicit operator Result<TValue>(TValue value) => Success(value);
}