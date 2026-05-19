using MGH.Core.Domain.Abstractions;
using MGH.Core.Domain.Abstractions.Events;
using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.Entities;

public abstract class AggregateRoot<T> :
    AuditableAggregateRoot<T>,
    IHasDomainEvent,
    IAggregateRoot<T>
{
    private readonly List<DomainEvent> _domainEvents = new();

    public IReadOnlyList<DomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();
    
    public virtual IEnumerable<DomainEvent> GetDomainEvents()
    {
        return _domainEvents;
    }
    
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new ArgumentNullException(nameof(domainEvent));

        if (_domainEvents.Contains(domainEvent))
            throw new InvalidOperationException(
                "The same domain event instance cannot be added more than once.");

        _domainEvents.Add(domainEvent);
        IncrementVersion();
    }

    #region Version
    private int Version { get; set; }

    public virtual void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public int GetVersion()
    {
        return Version;
    }

    private void IncrementVersion()
    {
        Version++;
    }

    #endregion
}