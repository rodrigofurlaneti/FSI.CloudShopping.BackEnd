namespace FSI.CloudShopping.Application.Sagas;

using FSI.CloudShopping.Application.Sagas.Steps;
using FSI.CloudShopping.Domain.Sagas;
using Microsoft.Extensions.Logging;

/// <summary>
/// Orchestrates the Order Checkout SAGA using the Orchestration pattern.
///
/// Pipeline:
///   Step 1 → ReserveStock        (compensation: ReleaseStock)
///   Step 2 → ProcessPayment      (compensation: RefundPayment)
///   Step 3 → ConfirmOrder        (compensation: CancelOrder)
///   Step 4 → SendNotification    (compensation: no-op)
///
/// ILogger lives here in the Application layer (not in Domain).
/// Logging is wired via overrides of the protected virtual hooks in SagaOrchestratorBase.
/// </summary>
public sealed class OrderCheckoutSagaOrchestrator : SagaOrchestratorBase<OrderCheckoutSagaState>
{
    private readonly ILogger<OrderCheckoutSagaOrchestrator> _logger;

    public OrderCheckoutSagaOrchestrator(
        ReserveStockStep reserveStock,
        ProcessPaymentStep processPayment,
        ConfirmOrderStep confirmOrder,
        SendNotificationStep sendNotification,
        ILogger<OrderCheckoutSagaOrchestrator> logger)
    {
        _logger = logger;

        // Registration order defines execution sequence
        AddStep(reserveStock);
        AddStep(processPayment);
        AddStep(confirmOrder);
        AddStep(sendNotification);
    }

    // ── Logging hooks — override the Domain base no-ops ──────────────────────

    protected override void OnSagaStarted(Guid sagaId, int stepCount)
        => _logger.LogInformation("SAGA [{SagaId}] starting with {StepCount} steps.", sagaId, stepCount);

    protected override void OnStepExecuting(Guid sagaId, string stepName)
        => _logger.LogInformation("SAGA [{SagaId}] executing step: {Step}", sagaId, stepName);

    protected override void OnStepCompleted(Guid sagaId, string stepName)
        => _logger.LogInformation("SAGA [{SagaId}] step '{Step}' completed.", sagaId, stepName);

    protected override void OnStepFailed(Guid sagaId, string stepName, Exception ex)
        => _logger.LogError(ex, "SAGA [{SagaId}] step '{Step}' failed. Starting compensation.", sagaId, stepName);

    protected override void OnCompensating(Guid sagaId, string stepName)
        => _logger.LogWarning("SAGA [{SagaId}] compensating step: {Step}", sagaId, stepName);

    protected override void OnCompensated(Guid sagaId, string stepName)
        => _logger.LogWarning("SAGA [{SagaId}] step '{Step}' compensated.", sagaId, stepName);

    protected override void OnCompensationFailed(Guid sagaId, string stepName, Exception ex)
        => _logger.LogCritical(ex, "SAGA [{SagaId}] COMPENSATION FAILED for step '{Step}'. Manual intervention required.", sagaId, stepName);

    protected override void OnSagaCompleted(Guid sagaId)
        => _logger.LogInformation("SAGA [{SagaId}] completed successfully.", sagaId);
}
