using MGH.Core.Domain.Base;

namespace MGH.Core.Domain.Entities;

public class OutboxMessage : FullAuditableEntity<Guid>
{
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = String.Empty;
    public DateTime? ProcessedAt { get; set; }
    public string Error { get; set; }
}