using System.Text.Json;

namespace MGH.Core.Infrastructure.Persistence.Entities;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Error { get; set; }
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;

    public void SerializePayload(object payload)
    {
        if (payload is null)
            throw new ArgumentNullException(nameof(payload));

        Type = payload.GetType().Name!;
        Payload = JsonSerializer.Serialize(payload);
    }

    public T DeserializePayloadAs<T>()
    {
        return JsonSerializer.Deserialize<T>(Payload)!;
    }

    public object DeserializePayload(Type type)
    {
        return JsonSerializer.Deserialize(Payload, type)!;
    }

    public void MarkAsProcessed()
    {
        ProcessedAt = DateTime.UtcNow;
    }
}