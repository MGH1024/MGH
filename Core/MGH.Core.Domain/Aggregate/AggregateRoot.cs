using MGH.Core.Domain.Entity.Base;

namespace MGH.Core.Domain.Aggregate;

public interface IAggregateRoot
{
    IEnumerable<DomainEvent> Events { get; }
    void ClearEvents();
}

public abstract class AggregateRoot<T> : BaseEntity<T>, IAuditAbleEntity, IAggregateRoot
{
    public int Version { get; set; }
    private bool _versionIncremented;

    public IEnumerable<DomainEvent> Events => _events;
    private readonly List<DomainEvent> _events = new();

    protected void AddEvent(DomainEvent @event)
    {
        if (!_events.Any() && !_versionIncremented)
        {
            Version++;
            _versionIncremented = true;
        }

        _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();

    protected void IncrementVersion()
    {
        if (_versionIncremented)
            return;
        Version++;
        _versionIncremented = true;
    }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}