namespace MGH.Core.Domain.Abstractions.Auditing;

public interface IDeletedAuditable
{
    DateTime? Deleted { get; set; }
    string? DeletedBy { get; set; }
    string? DeletedFromIp { get; set; }
    bool IsDeleted => Deleted.HasValue;
}
