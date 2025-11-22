using System.Text;
using System.Text.Json;
using MGH.Core.Domain.Events;

namespace MGH.Core.CrossCutting.JsonHelpers
{
    /// <summary>
    /// Provides JSON serialization and deserialization helpers for EventBus events.
    /// </summary>
    public static class EventBusJsonHelper
    {
        /// <summary>
        /// Serializes an <see cref="IEvent"/> for EventBus transport.
        /// For <see cref="DomainEvent"/>, only its envelope properties 
        /// (EventData, EventOrder, Id, OccurredOn) are included.
        /// </summary>
        /// <param name="eventModel">The event to serialize.</param>
        /// <returns>A UTF-8 encoded byte array representing the serialized event.</returns>
        public static byte[] SerializeEventBusEvent(IEvent eventModel)
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

        /// <summary>
        /// Deserializes a byte array from EventBus into an event of type <typeparamref name="T"/>.
        /// Handles both plain events and <see cref="DomainEvent"/> envelopes containing EventData.
        /// </summary>
        /// <typeparam name="T">The type of event to deserialize.</typeparam>
        /// <param name="messageBytes">The UTF-8 byte array containing the serialized event.</param>
        /// <returns>The deserialized event of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="messageBytes"/> is null or empty.</exception>
        public static T DeserializeEventBusEvent<T>(byte[] messageBytes) where T : IEvent
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
