using MGH.Core.Domain.BaseEntity;

namespace MGH.Core.Domain.Entity.Outboxes;

public class OutboxMessage : AuditAbleEntity<Guid>
{
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = String.Empty;
    public DateTime? ProcessedAt { get; set; }
    public string Error { get; set; }
}