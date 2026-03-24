namespace FSI.CloudShopping.Application.Common;

/// <summary>
/// Result pattern implementation for command/query handlers to return either success or failure.
/// </summary>
public abstract class Result
{
    public sealed class Success : Result
    {
        public object? Data { get; }

        public Success(object? data = null)
        {
            Data = data;
        }
    }

    public sealed class Failure : Result
    {
        public IReadOnlyList<string> Errors { get; }

        public Failure(params string[] errors)
        {
            Errors = errors.ToList().AsReadOnly();
        }

        public Failure(IEnumerable<string> errors)
        {
            Errors = errors.ToList().AsReadOnly();
        }
    }
}

public abstract class Result<T> : Result
{
    public sealed class Success : Result<T>
    {
        public T Value { get; }

        public Success(T value)
        {
            Value = value;
        }
    }

    public sealed class Failure : Result<T>
    {
        public IReadOnlyList<string> Errors { get; }

        public Failure(params string[] errors)
        {
            Errors = errors.ToList().AsReadOnly();
        }

        public Failure(IEnumerable<string> errors)
        {
            Errors = errors.ToList().AsReadOnly();
        }
    }

    public bool IsSuccess => this is Success;
    public bool IsFailure => this is Failure;

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<IReadOnlyList<string>, TResult> onFailure)
    {
        return this switch
        {
            Success success => onSuccess(success.Value),
            Failure failure => onFailure(failure.Errors),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }

    public async Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> onSuccess,
        Func<IReadOnlyList<string>, Task<TResult>> onFailure)
    {
        return this switch
        {
            Success success => await onSuccess(success.Value),
            Failure failure => await onFailure(failure.Errors),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }

    public void Match(
        Action<T> onSuccess,
        Action<IReadOnlyList<string>> onFailure)
    {
        switch (this)
        {
            case Success success:
                onSuccess(success.Value);
                break;
            case Failure failure:
                onFailure(failure.Errors);
                break;
        }
    }

    public Result<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
        return this switch
        {
            Success success => new Result<TResult>.Success(mapper(success.Value)),
            Failure failure => new Result<TResult>.Failure(failure.Errors),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }
}
