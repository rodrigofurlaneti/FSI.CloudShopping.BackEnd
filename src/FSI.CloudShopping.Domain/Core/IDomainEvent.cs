namespace FSI.CloudShopping.Domain.Core;

using MediatR;

/// <summary>
/// Marker interface for domain events. Domain events are immutable and represent something
/// that happened in the domain. They are handled by MediatR.
/// </summary>
public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}

/// <summary>
/// Base implementation of domain event.
/// </summary>
public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
