using MGH.Core.Domain.Abstracts;

namespace MGH.Core.Domain.Concretes;

public class AuditableEntity<TId> : IEntity<TId>, IAuditable
{
    public AuditableEntity()
    {
        Id = default!;
    }

    public AuditableEntity(TId id)
    {
        Id = id;
    }
    
    
    public TId Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}