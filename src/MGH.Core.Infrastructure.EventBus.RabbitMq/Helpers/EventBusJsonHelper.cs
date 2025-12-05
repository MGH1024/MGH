using System.Text;
using System.Text.Json;
using MGH.Core.Domain.Events;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Helpers
{
    internal static class EventBusJsonHelper
    {
        internal static byte[] SerializeEventBusEvent(IEvent eventModel)
        {
            object payload = eventModel switch
            {
                DomainEvent domainEvent => new
                {
                    domainEvent.EventData,
                    domainEvent.EventOrder,
                    domainEvent.Id,
                    domainEvent.OccurredOn
                },
                _ => eventModel
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Encoding.UTF8.GetBytes(json);
        }

        internal static T DeserializeEventBusEvent<T>(byte[] messageBytes) where T : IEvent
        {
            if (messageBytes == null || messageBytes.Length == 0)
                throw new ArgumentException("Message bytes cannot be null or empty.", nameof(messageBytes));

            var json = Encoding.UTF8.GetString(messageBytes);

            try
            {
                // Attempt to parse as DomainEvent envelope
                var envelope = JsonSerializer.Deserialize<DomainEventEnvelope>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (envelope != null && envelope.EventData.ValueKind != JsonValueKind.Undefined)
                {
                    // Deserialize the actual event from EventData
                    return JsonSerializer.Deserialize<T>(envelope.EventData.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })!;
                }
            }
            catch
            {
                // Fallback to direct deserialization
            }

            // Fallback: deserialize directly
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }
}
