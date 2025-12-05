using System.Text.Json.Serialization;

namespace MGH.Core.Infrastructure.HealthCheck.QueueHealthCheck;

public partial class GarbageCollection
{
     [JsonPropertyName("fullsweep_after")]
    public long FullsweepAfter { get; set; }

     [JsonPropertyName("max_heap_size")]
    public long MaxHeapSize { get; set; }

     [JsonPropertyName("min_bin_vheap_size")]
    public long MinBinVheapSize { get; set; }

     [JsonPropertyName("min_heap_size")]
    public long MinHeapSize { get; set; }

     [JsonPropertyName("minor_gcs")]
    public long MinorGcs { get; set; }
}


