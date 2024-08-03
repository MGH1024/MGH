namespace MGH.Core.Domain.Entity.Base;

public class AuditAbleEntity<TId>(TId id) : IEntity<TId>, IAuditAbleEntity
{
    public AuditAbleEntity() : this(default!)
    {
    }

    public TId Id { get; set; } = id;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}