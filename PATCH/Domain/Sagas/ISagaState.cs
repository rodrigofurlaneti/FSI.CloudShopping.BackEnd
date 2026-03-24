namespace FSI.CloudShopping.Domain.Sagas;

/// <summary>
/// Marker interface for SAGA state objects.
/// The state is the shared context passed through all saga steps.
/// It accumulates results and is used by compensation logic.
/// </summary>
public interface ISagaState
{
    Guid SagaId { get; }
    SagaStatus Status { get; set; }
    string? FailureReason { get; set; }
    DateTime StartedAt { get; }
    DateTime? CompletedAt { get; set; }
}

/// <summary>
/// Lifecycle status of a SAGA execution.
/// </summary>
public enum SagaStatus
{
    NotStarted = 0,
    Running = 1,
    Completed = 2,
    Compensating = 3,
    Compensated = 4,
    Failed = 5
}
