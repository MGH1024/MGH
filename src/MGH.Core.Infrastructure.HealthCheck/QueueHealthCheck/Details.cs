using System.Text.Json.Serialization;

namespace MGH.Core.Infrastructure.HealthCheck.QueueHealthCheck;


public partial class Details
{
     [JsonPropertyName("rate")]
    public double Rate { get; set; }
}


