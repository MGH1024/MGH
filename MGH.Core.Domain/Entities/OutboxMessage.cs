namespace MGH.Core.Domain.Entities;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Error { get; set; }
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Type { get; set; } = string.Empty;
    public object Payload { get; set; } = string.Empty;
}