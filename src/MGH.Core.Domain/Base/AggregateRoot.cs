using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.Base;
public abstract class AggregateRoot<T> : 
    FullAuditableEntity<T>,
    IDomainEvent, 
    IAggregateRoot<T>
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents 
        => _domainEvents.AsReadOnly();


    public virtual IEnumerable<DomainEvent> GetDomainEvents()
    {
        return _domainEvents;
    }

    public virtual void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected int Version { get; set; } = 0;

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new ArgumentNullException(nameof(domainEvent));

        if (domainEvent.EventData is null)
            throw new ArgumentException(
                "Domain event data cannot be null.",
                nameof(domainEvent));

        if (_domainEvents.Contains(domainEvent))
            throw new InvalidOperationException(
                "The same domain event instance cannot be added more than once.");

        _domainEvents.Add(domainEvent);
        IncrementVersion();
    }

    private void IncrementVersion()
    {
        Version++;
    }
}
