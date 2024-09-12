using MGH.Core.Domain.Entity.Base;

namespace MGH.Core.Domain.Outboxes;

public class OutboxMessage : AuditAbleEntity<Guid>
{
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = String.Empty;
    public DateTime? ProcessedAt { get; set; }
    public string Error { get; set; }
}