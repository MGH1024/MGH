using MGH.Core.Domain.Audits;

namespace MGH.Core.Domain.Base;

public abstract class FullAuditableEntity<T> : IEntity<T>,IFullAuditable
{
    public T Id { get; set; }
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
