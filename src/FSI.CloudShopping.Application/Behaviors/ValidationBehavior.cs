namespace FSI.CloudShopping.Application.Behaviors;

using FluentValidation;
using MediatR;
using FSI.CloudShopping.Application.Common;

/// <summary>
/// MediatR pipeline behavior that validates requests using FluentValidation.
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .Select(error => error.ErrorMessage)
            .ToList();

        if (failures.Count != 0)
        {
            return (dynamic)new Result.Failure(failures);
        }

        return await next();
    }
}

/// <summary>
/// MediatR pipeline behavior that validates requests using FluentValidation for generic Result<T> responses.
/// </summary>
public class ValidationBehavior<TRequest, TResponse, T> : IPipelineBehavior<TRequest, Result<T>>
    where TRequest : IRequest<Result<T>>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<Result<T>> Handle(TRequest request, RequestHandlerDelegate<Result<T>> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .Select(error => error.ErrorMessage)
            .ToList();

        if (failures.Count != 0)
        {
            return new Result<T>.Failure(failures);
        }

        return await next();
    }
}
