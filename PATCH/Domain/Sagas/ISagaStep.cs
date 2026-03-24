namespace FSI.CloudShopping.Domain.Sagas;

/// <summary>
/// Represents a single, compensable step in a SAGA transaction.
/// Each step must define both the action (Execute) and its compensating action (Compensate)
/// to support the rollback mechanism when a downstream step fails.
/// </summary>
/// <typeparam name="TState">The shared state object passed across all saga steps.</typeparam>
public interface ISagaStep<TState> where TState : ISagaState
{
    /// <summary>Name of this step, used for logging and observability.</summary>
    string StepName { get; }

    /// <summary>
    /// Executes the step's primary action. Mutates <paramref name="state"/> as needed.
    /// </summary>
    Task ExecuteAsync(TState state, CancellationToken cancellationToken = default);

    /// <summary>
    /// Compensating transaction. Invoked when a later step fails, rolling back this step's effects.
    /// Must be idempotent.
    /// </summary>
    Task CompensateAsync(TState state, CancellationToken cancellationToken = default);
}
