using System.Text.Json;

namespace MGH.Core.CrossCutting.JsonHelpers
{
    public  class DomainEventEnvelope
    {
        public JsonElement EventData { get; set; }
        public int EventOrder { get; set; }
        public Guid Id { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
