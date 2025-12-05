using System.Text.Json.Serialization;

namespace MGH.Core.Infrastructure.HealthCheck.QueueHealthCheck;

public partial class QueueDetailsResult
{

     [JsonPropertyName("messages")]
    public long Messages { get; set; }

     [JsonPropertyName("consumer_details")]
    public object[] ConsumerDetails { get; set; }

     [JsonPropertyName("arguments")]
    public Arguments Arguments { get; set; }

     [JsonPropertyName("auto_delete")]
    public bool AutoDelete { get; set; }

     [JsonPropertyName("consumer_capacity")]
    public long ConsumerCapacity { get; set; }

     [JsonPropertyName("consumer_utilisation")]
    public long ConsumerUtilisation { get; set; }

     [JsonPropertyName("consumers")]
    public long Consumers { get; set; }

     [JsonPropertyName("deliveries")]
    public object[] Deliveries { get; set; }

     [JsonPropertyName("durable")]
    public bool Durable { get; set; }

     [JsonPropertyName("effective_policy_definition")]
    public EffectivePolicyDefinition EffectivePolicyDefinition { get; set; }

     [JsonPropertyName("exclusive")]
    public bool Exclusive { get; set; }

     [JsonPropertyName("exclusive_consumer_tag")]
    public object ExclusiveConsumerTag { get; set; }

     [JsonPropertyName("garbage_collection")]
    public GarbageCollection GarbageCollection { get; set; }

     [JsonPropertyName("head_message_timestamp")]
    public object HeadMessageTimestamp { get; set; }

     [JsonPropertyName("idle_since")]
    public DateTimeOffset IdleSince { get; set; }

     [JsonPropertyName("incoming")]
    public object[] Incoming { get; set; }

     [JsonPropertyName("memory")]
    public long Memory { get; set; }

     [JsonPropertyName("message_bytes")]
    public long MessageBytes { get; set; }

     [JsonPropertyName("message_bytes_paged_out")]
    public long MessageBytesPagedOut { get; set; }

     [JsonPropertyName("message_bytes_persistent")]
    public long MessageBytesPersistent { get; set; }

     [JsonPropertyName("message_bytes_ram")]
    public long MessageBytesRam { get; set; }

     [JsonPropertyName("message_bytes_ready")]
    public long MessageBytesReady { get; set; }

     [JsonPropertyName("message_bytes_unacknowledged")]
    public long MessageBytesUnacknowledged { get; set; }

     [JsonPropertyName("message_stats")]
    public MessageStats MessageStats { get; set; }



     [JsonPropertyName("messages_details")]
    public Details MessagesDetails { get; set; }

     [JsonPropertyName("messages_paged_out")]
    public long MessagesPagedOut { get; set; }

     [JsonPropertyName("messages_persistent")]
    public long MessagesPersistent { get; set; }

     [JsonPropertyName("messages_ram")]
    public long MessagesRam { get; set; }

     [JsonPropertyName("messages_ready")]
    public long MessagesReady { get; set; }

     [JsonPropertyName("messages_ready_details")]
    public Details MessagesReadyDetails { get; set; }

     [JsonPropertyName("messages_ready_ram")]
    public long MessagesReadyRam { get; set; }

     [JsonPropertyName("messages_unacknowledged")]
    public long MessagesUnacknowledged { get; set; }

     [JsonPropertyName("messages_unacknowledged_details")]
    public Details MessagesUnacknowledgedDetails { get; set; }

     [JsonPropertyName("messages_unacknowledged_ram")]
    public long MessagesUnacknowledgedRam { get; set; }

     [JsonPropertyName("name")]
    public string Name { get; set; }

     [JsonPropertyName("node")]
    public string Node { get; set; }

     [JsonPropertyName("operator_policy")]
    public object OperatorPolicy { get; set; }

     [JsonPropertyName("policy")]
    public object Policy { get; set; }

     [JsonPropertyName("recoverable_slaves")]
    public object RecoverableSlaves { get; set; }

     [JsonPropertyName("reductions")]
    public long Reductions { get; set; }

     [JsonPropertyName("reductions_details")]
    public Details ReductionsDetails { get; set; }

     [JsonPropertyName("single_active_consumer_tag")]
    public object SingleActiveConsumerTag { get; set; }

     [JsonPropertyName("state")]
    public string State { get; set; }

     [JsonPropertyName("type")]
    public string Type { get; set; }

     [JsonPropertyName("vhost")]
    public string Vhost { get; set; }
}


