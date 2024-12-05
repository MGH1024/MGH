namespace MGH.Core.Domain.BaseEntity.Abstract;

public interface IAuditAbleEntity
{
    DateTime CreatedAt { get; set; }

    string CreatedBy { get;set; }
    string CreatedByIp { get; set; }

    DateTime? UpdatedAt { get;set; }

    string UpdatedBy { get;set; }
    string UpdatedByIp { get; set; }

    DateTime? DeletedAt { get;set; }

    string DeletedBy { get;set; }
    string DeletedByIp { get; set; }
}