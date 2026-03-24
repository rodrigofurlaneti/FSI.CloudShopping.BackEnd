namespace FSI.CloudShopping.Application.Sagas;

using FSI.CloudShopping.Domain.Sagas;
using Microsoft.Extensions.Logging;

/// <summary>
/// Abstract base orchestrator implementing the SAGA (Orchestration) pattern.
/// Lives in the Application layer so it can reference Microsoft.Extensions.Logging
/// without polluting the Domain layer with infrastructure concerns.
///
/// Subclasses register steps in their constructor via AddStep().
///
/// Execution flow:
///   Step1.Execute → Step2.Execute → … → StepN.Execute
///   On failure at StepK:
///   StepK-1.Compensate → … → Step1.Compensate  (reverse order, guaranteed)
/// </summary>
public abstract class SagaOrchestratorBase<TState> : ISagaOrchestrator<TState>
    where TState : ISagaState
{
    private readonly List<ISagaStep<TState>> _steps = [];
    private readonly ILogger _logger;

    protected SagaOrchestratorBase(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Registers a step in the pipeline. Call from the subclass constructor — order matters.
    /// </summary>
    protected void AddStep(ISagaStep<TState> step) => _steps.Add(step);

    public async Task<TState> ExecuteAsync(TState state, CancellationToken cancellationToken = default)
    {
        state.Status = SagaStatus.Running;
        var completedSteps = new Stack<ISagaStep<TState>>();

        _logger.LogInformation("SAGA [{SagaId}] starting with {StepCount} steps.", state.SagaId, _steps.Count);

        foreach (var step in _steps)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                _logger.LogInformation("SAGA [{SagaId}] executing step: {Step}", state.SagaId, step.StepName);
                await step.ExecuteAsync(state, cancellationToken);
                completedSteps.Push(step);
                _logger.LogInformation("SAGA [{SagaId}] step '{Step}' completed.", state.SagaId, step.StepName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SAGA [{SagaId}] step '{Step}' failed. Starting compensation.", state.SagaId, step.StepName);
                state.Status = SagaStatus.Compensating;
                state.FailureReason = ex.Message;
                await CompensateAsync(state, completedSteps, cancellationToken);
                return state;
            }
        }

        state.Status = SagaStatus.Completed;
        state.CompletedAt = DateTime.UtcNow;
        _logger.LogInformation("SAGA [{SagaId}] completed successfully.", state.SagaId);
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
                _logger.LogWarning("SAGA [{SagaId}] compensating step: {Step}", state.SagaId, step.StepName);
                await step.CompensateAsync(state, cancellationToken);
                _logger.LogWarning("SAGA [{SagaId}] step '{Step}' compensated.", state.SagaId, step.StepName);
            }
            catch (Exception ex)
            {
                // Compensation failure → dead-letter queue / manual intervention required.
                _logger.LogCritical(
                    ex,
                    "SAGA [{SagaId}] COMPENSATION FAILED for step '{Step}'. Manual intervention required.",
                    state.SagaId, step.StepName);
            }
        }

        state.Status = SagaStatus.Compensated;
        state.CompletedAt = DateTime.UtcNow;
    }
}
