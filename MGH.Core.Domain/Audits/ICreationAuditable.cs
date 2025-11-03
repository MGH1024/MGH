namespace MGH.Core.Domain.Audits;

public interface ICreationAuditable
{
    DateTime CreatedAt { get; set; }
    string CreatedBy { get; set; }
    string CreatedByIp { get; set; }
}
