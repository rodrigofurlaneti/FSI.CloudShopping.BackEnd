namespace FSI.CloudShopping.Domain.Core;

/// <summary>
/// Base class for aggregate roots. Adds domain event management to entities.
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    protected AggregateRoot(TId id) : base(id)
    {
    }

    protected AggregateRoot()
    {
    }

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        AddDomainEvent(domainEvent);
    }

    public void ClearEvents()
    {
        ClearDomainEvents();
    }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents()
    {
        return DomainEvents;
    }
}
