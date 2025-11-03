namespace MGH.Core.Domain.Audits;

public interface IDeletionAuditable
{
    DateTime? DeletedAt { get; set; }
    string DeletedBy { get; set; }
    string DeletedByIp { get; set; }

    bool IsDeleted => DeletedAt.HasValue;
}
