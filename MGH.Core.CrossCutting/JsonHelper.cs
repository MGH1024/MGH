using System.Text;
using System.Text.Json;
using MGH.Core.Domain.Events;

namespace MGH.Core.CrossCutting
{
    public static class JsonHelper
    {
        public static byte[] SerializeEvent(IEvent model)
        {
            // If it's a DomainEvent, select only EventData, EventOrder, Id, OccurredOn
            object payload = model switch
            {
                DomainEvent domainEvent => new
                {
                    domainEvent.EventData,
                    domainEvent.EventOrder,
                    domainEvent.Id,
                    domainEvent.OccurredOn
                },
                _ => model
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Encoding.UTF8.GetBytes(json);
        }
    }
}
