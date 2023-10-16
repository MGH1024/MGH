using MGH.Domain.Abstracts;

namespace MGH.Domain.Concretes;

public class AuditableEntity<T> : IEntity<T>, IAuditable
{
    public AuditableEntity()
    {
        CreatedAt = DateTime.Now;
    }
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}