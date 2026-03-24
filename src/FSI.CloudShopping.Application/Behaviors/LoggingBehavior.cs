namespace FSI.CloudShopping.Application.Behaviors;

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// MediatR pipeline behavior that logs request and response information with timing.
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling request: {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        _logger.LogInformation("Completed request: {RequestName} in {ElapsedMilliseconds}ms",
            requestName, stopwatch.ElapsedMilliseconds);

        return response;
    }
}
