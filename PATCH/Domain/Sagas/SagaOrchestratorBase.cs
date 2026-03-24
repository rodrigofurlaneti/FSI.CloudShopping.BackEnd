namespace FSI.CloudShopping.Domain.Sagas;

/// <summary>
/// Abstract base orchestrator that implements the SAGA (Orchestration) pattern
/// with automatic compensation — without any external infrastructure dependency.
///
/// The Domain layer must remain pure (no Microsoft.Extensions.Logging).
/// Logging hooks are exposed as protected virtual methods so the Application-layer
/// subclass can inject ILogger and override them without coupling this class to infra.
///
/// Execution flow:
///   Step1.Execute → Step2.Execute → ... → StepN.Execute
///   On failure at StepK:
///   StepK-1.Compensate → ... → Step1.Compensate  (reverse order, guaranteed)
/// </summary>
public abstract class SagaOrchestratorBase<TState> : ISagaOrchestrator<TState>
    where TState : ISagaState
{
    private readonly List<ISagaStep<TState>> _steps = [];

    /// <summary>Registers a step. Call from the subclass constructor — order matters.</summary>
    protected void AddStep(ISagaStep<TState> step) => _steps.Add(step);

    // ── Logging hooks (no-op by default, override in Application layer) ───────
    protected virtual void OnSagaStarted(Guid sagaId, int stepCount) { }
    protected virtual void OnStepExecuting(Guid sagaId, string stepName) { }
    protected virtual void OnStepCompleted(Guid sagaId, string stepName) { }
    protected virtual void OnStepFailed(Guid sagaId, string stepName, Exception ex) { }
    protected virtual void OnCompensating(Guid sagaId, string stepName) { }
    protected virtual void OnCompensated(Guid sagaId, string stepName) { }
    protected virtual void OnCompensationFailed(Guid sagaId, string stepName, Exception ex) { }
    protected virtual void OnSagaCompleted(Guid sagaId) { }

    public async Task<TState> ExecuteAsync(TState state, CancellationToken cancellationToken = default)
    {
        state.Status = SagaStatus.Running;
        var completedSteps = new Stack<ISagaStep<TState>>();

        OnSagaStarted(state.SagaId, _steps.Count);

        foreach (var step in _steps)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                OnStepExecuting(state.SagaId, step.StepName);
                await step.ExecuteAsync(state, cancellationToken);
                completedSteps.Push(step);
                OnStepCompleted(state.SagaId, step.StepName);
            }
            catch (Exception ex)
            {
                OnStepFailed(state.SagaId, step.StepName, ex);
                state.Status = SagaStatus.Compensating;
                state.FailureReason = ex.Message;
                await CompensateAsync(state, completedSteps, cancellationToken);
                return state;
            }
        }

        state.Status = SagaStatus.Completed;
        state.CompletedAt = DateTime.UtcNow;
        OnSagaCompleted(state.SagaId);
        return state;
    }

    private async Task CompensateAsync(
        TState state,
        Stack<ISagaStep<TState>> completedSteps,
        CancellationToken cancellationToken)
    {
        while (completedSteps.Count > 0)
        {
            var step = completedSteps.Pop();
            try
            {
                OnCompensating(state.SagaId, step.StepName);
                await step.CompensateAsync(state, cancellationToken);
                OnCompensated(state.SagaId, step.StepName);
            }
            catch (Exception ex)
            {
                // Compensation failure: log and continue rolling back remaining steps.
                // In production, publish to a dead-letter queue for manual intervention.
                OnCompensationFailed(state.SagaId, step.StepName, ex);
            }
        }

        state.Status = SagaStatus.Compensated;
        state.CompletedAt = DateTime.UtcNow;
    }
}
