namespace MGH.Core.Domain.Audits;

public interface IModificationAuditable
{
    DateTime? UpdatedAt { get; set; }
    string UpdatedBy { get; set; }
    string UpdatedByIp { get; set; }
}
