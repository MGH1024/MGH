using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.BaseModels;


public abstract class Aggregate<T> : Entity<T>, IAggregate<T>
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public int Version { get; set; } = 0;

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
        IncrementVersion();
    }

    public IEvent[] ClearDomainEvents()
    {
        IEvent[] dequeuedEvents = _domainEvents.ToArray();
        _domainEvents.Clear();
        return dequeuedEvents;
    }

    private void IncrementVersion()
    {
        Version++;
    }
}
