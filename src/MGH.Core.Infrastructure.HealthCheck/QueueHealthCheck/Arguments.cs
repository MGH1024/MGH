using System.Text.Json.Serialization;

namespace MGH.Core.Infrastructure.HealthCheck.QueueHealthCheck;

public partial class Arguments
{
     [JsonPropertyName("x-dead-letter-exchange")]
    public string XDeadLetterExchange { get; set; }

     [JsonPropertyName("x-dead-letter-routing-key")]
    public string XDeadLetterRoutingKey { get; set; }

     [JsonPropertyName("x-message-ttl")]
    public long XMessageTtl { get; set; }
}



