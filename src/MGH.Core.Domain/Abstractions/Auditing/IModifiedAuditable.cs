namespace MGH.Core.Domain.Abstractions.Auditing;

public interface IModifiedAuditable
{
    DateTime? Modified { get; set; }
    string? ModifiedBy { get; set; }
    string? ModifiedFromIp { get; set; }
    bool IsModified => Modified.HasValue; 
}
