using MGH.Core.Domain.BaseModels;

namespace MGH.Core.Domain.Entities;

public class OutboxMessage : Entity<Guid>
{
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = String.Empty;
    public DateTime? ProcessedAt { get; set; }
    public string Error { get; set; }
}