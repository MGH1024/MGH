using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.Base;
public abstract class AggregateRoot<T> : FullAuditableEntity<T>, IGeneratesDomainEvent, IAggregateRoot<T>
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private readonly List<DomainEvent> _integratedEvents = new();
    public IReadOnlyList<DomainEvent> IntegratedEvents => _integratedEvents.AsReadOnly();


    public virtual IEnumerable<DomainEvent> GetDomainEvents()
    {
        return _domainEvents;
    }

    public virtual IEnumerable<DomainEvent> GetIntegratedEvents()
    {
        return _integratedEvents;
    }

    public virtual void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public virtual void ClearIntegratedEvents()
    {
        _integratedEvents.Clear();
    }

    public int Version { get; set; } = 0;


    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
        IncrementVersion();
    }

    protected virtual void AddDomainEvent(object eventData)
    {
        if (eventData == null)
            throw new ArgumentNullException(nameof(eventData));
        _domainEvents.Add(new DomainEvent(eventData));
    }

    protected virtual void AddIntegratedEvent(object eventData)
    {
        if (eventData == null)
            throw new ArgumentNullException(nameof(eventData));
        _integratedEvents.Add(new DomainEvent(eventData));
    }

    private void IncrementVersion()
    {
        Version++;
    }
}
