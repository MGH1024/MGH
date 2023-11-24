using MGH.Core.Domain.Abstracts;

namespace MGH.Core.Domain.Concretes;

public abstract class AggregateRoot<T> : BaseEntity<T>, IAuditable
{
    public int Version { get; set; }
    private bool _versionIncremented;

    public IEnumerable<IDomainEvent> Events => _events;
    private readonly List<IDomainEvent> _events = new();

    protected void AddEvent(IDomainEvent @event)
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