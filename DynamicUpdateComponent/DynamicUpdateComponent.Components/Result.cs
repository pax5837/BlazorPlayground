namespace DynamicUpdateComponent.Components;

internal class Result<TSuccess, TError>
{
    private TSuccess? success;

    private TError? error;

    public bool IsSuccess { get; private set; }

    private Result(TSuccess? success, TError? error, bool isSuccess)
    {
        IsSuccess = isSuccess;
        this.success = success;
        this.error = error;
    }

    public void Match(Action<TSuccess> whenSuccess, Action<TError> whenError)
    {
        if (IsSuccess)
        {
            whenSuccess(success!);
        }
        else
        {
            whenError(error!);
        }
    }

    public T Switch<T>(Func<TSuccess, T> whenSuccess, Func<TError, T> whenError)
    {
        if (IsSuccess)
        {
            return whenSuccess(success!);
        }
        else
        {
            return whenError(error!);
        }
    }

    public static Result<TSuccess, TError> FromSuccess(TSuccess success)
    {
        return new Result<TSuccess, TError>(success, default, true);
    }

    public static Result<TSuccess, TError> FromError(TError error)
    {
        return new Result<TSuccess, TError>(default, error, true);
    }
}