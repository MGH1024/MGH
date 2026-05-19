namespace MGH.Core.Domain.Abstractions.Auditing;

public interface ICreatedAuditable
{
    DateTime Created { get; set; }
    string? CreatedBy { get; set; }
    string? CreatedFromIp { get; set; }
}
