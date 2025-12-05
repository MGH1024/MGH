using System.Text.Json.Serialization;

namespace MGH.Core.Infrastructure.HealthCheck.QueueHealthCheck;

public partial class MessageStats
{
     [JsonPropertyName("ack")]
    public long Ack { get; set; }

     [JsonPropertyName("ack_details")]
    public Details AckDetails { get; set; }

     [JsonPropertyName("deliver")]
    public long Deliver { get; set; }

     [JsonPropertyName("deliver_details")]
    public Details DeliverDetails { get; set; }

     [JsonPropertyName("deliver_get")]
    public long DeliverGet { get; set; }

     [JsonPropertyName("deliver_get_details")]
    public Details DeliverGetDetails { get; set; }

     [JsonPropertyName("deliver_no_ack")]
    public long DeliverNoAck { get; set; }

     [JsonPropertyName("deliver_no_ack_details")]
    public Details DeliverNoAckDetails { get; set; }

     [JsonPropertyName("get")]
    public long Get { get; set; }

     [JsonPropertyName("get_details")]
    public Details GetDetails { get; set; }

     [JsonPropertyName("get_empty")]
    public long GetEmpty { get; set; }

     [JsonPropertyName("get_empty_details")]
    public Details GetEmptyDetails { get; set; }

     [JsonPropertyName("get_no_ack")]
    public long GetNoAck { get; set; }

     [JsonPropertyName("get_no_ack_details")]
    public Details GetNoAckDetails { get; set; }

     [JsonPropertyName("publish")]
    public long Publish { get; set; }

     [JsonPropertyName("publish_details")]
    public Details PublishDetails { get; set; }

     [JsonPropertyName("redeliver")]
    public long Redeliver { get; set; }

     [JsonPropertyName("redeliver_details")]
    public Details RedeliverDetails { get; set; }
}


