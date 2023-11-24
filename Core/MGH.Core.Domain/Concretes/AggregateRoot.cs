using MGH.Core.Domain.Abstracts;

namespace MGH.Core.Domain.Concretes;

public abstract class AggregateRoot<T> : BaseEntity<T>
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
}