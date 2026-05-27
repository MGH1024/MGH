using System.Text;
using System.Text.Json;
using MGH.Core.Domain.Abstractions.Events;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Helpers
{
    internal static class EventBusJsonHelper
    {
        internal static byte[] SerializeEventBusEvent(IEventMetadata eventMetadataModel)
        {
            object envelope;
            if (eventMetadataModel is IDomainEvent domainEvent)
            {
                envelope = new DomainEventEnvelope
                {
                    Id = domainEvent.Id,
                    OccurredOn = domainEvent.OccurredOn,
                    EventData = JsonSerializer.SerializeToElement(domainEvent),
                };
            }
            else
            {
                envelope = eventMetadataModel;
            }

            var json = JsonSerializer.Serialize(envelope, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Encoding.UTF8.GetBytes(json);
        }

        internal static T DeserializeEventBusEvent<T>(byte[] messageBytes) where T : IEventMetadata
        {
            if (messageBytes == null || messageBytes.Length == 0)
                throw new ArgumentException("Message bytes cannot be null or empty.", nameof(messageBytes));

            var json = Encoding.UTF8.GetString(messageBytes);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Try to parse the JSON as a DomainEventEnvelope
            DomainEventEnvelope? envelope = null;
            try
            {
                envelope = JsonSerializer.Deserialize<DomainEventEnvelope>(json, options);
            }
            catch (JsonException)
            {
                // The JSON structure is not an envelope – we'll fall back to direct deserialization
            }

            // If the envelope was parsed and contains event data, deserialize the inner event
            if (envelope != null && envelope.EventData.ValueKind != JsonValueKind.Undefined)
            {
                return JsonSerializer.Deserialize<T>(envelope.EventData.GetRawText(), options)!;
            }

            // Fallback: the message is the event itself, not wrapped in an envelope
            return JsonSerializer.Deserialize<T>(json, options)!;
        }
    }
}