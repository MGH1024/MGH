using MGH.Core.Domain.Abstractions;
using MGH.Core.Domain.Abstractions.Auditing;

namespace MGH.Core.Domain.Entities;

public abstract class AuditableAggregateRoot<T> : IFullAuditable, IEntity<T>
{
    public T Id { get; protected set; }
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedFromIp { get; set; }
    public DateTime? Deleted { get; set; }
    public string? DeletedBy { get; set; }
    public string? DeletedFromIp { get; set; }
    public DateTime? Modified { get; set; }
    public string? ModifiedBy { get; set; }
    public string? ModifiedFromIp { get; set; }

    protected AuditableAggregateRoot(T id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    protected AuditableAggregateRoot()
    {
    }

    T IEntity<T>.Id
    {
        get => Id;
        set => Id = value;
    }
}