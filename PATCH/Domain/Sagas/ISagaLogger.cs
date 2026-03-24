namespace FSI.CloudShopping.Domain.Sagas;

/// <summary>
/// Lightweight logging abstraction for the Domain SAGA layer.
/// The Domain must NOT depend on Microsoft.Extensions.Logging directly.
/// The Application/Infrastructure layer supplies the concrete implementation
/// (backed by ILogger&lt;T&gt;) via Dependency Injection.
/// </summary>
public interface ISagaLogger
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? exception = null);
    void LogCritical(string message, Exception? exception = null);
}
