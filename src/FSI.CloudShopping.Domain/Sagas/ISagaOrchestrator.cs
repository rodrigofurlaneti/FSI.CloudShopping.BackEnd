namespace FSI.CloudShopping.Domain.Sagas;

/// <summary>
/// Contract for saga orchestrators.
/// An orchestrator executes steps sequentially and triggers compensation
/// in reverse order upon failure — guaranteeing eventual consistency.
/// </summary>
/// <typeparam name="TState">Shared saga state.</typeparam>
public interface ISagaOrchestrator<TState> where TState : ISagaState
{
    /// <summary>
    /// Executes the saga from the first step to the last.
    /// On any step failure, rolls back all previously completed steps in reverse order.
    /// </summary>
    Task<TState> ExecuteAsync(TState initialState, CancellationToken cancellationToken = default);
}
