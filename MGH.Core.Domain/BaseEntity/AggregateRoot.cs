using MGH.Core.Domain.BaseEntity.Abstract;

namespace MGH.Core.Domain.BaseEntity;


public abstract class AggregateRoot<T> : BaseEntity<T>, IAuditAbleEntity, IAggregateRoot
{
    public int Version { get; private set; }

    private readonly List<DomainEvent> _events = new();
    public IReadOnlyCollection<DomainEvent> Events => _events.AsReadOnly();

    protected void AddEvent(DomainEvent @event)
    {
        _events.Add(@event);
        IncrementVersion();
    }

    public void ClearEvents() => _events.Clear();

    private void IncrementVersion()
    {
        Version++;
    }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public string UpdatedByIp { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
    public string DeletedByIp { get; set; }
}
