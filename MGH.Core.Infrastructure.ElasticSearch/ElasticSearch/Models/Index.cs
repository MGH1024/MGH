namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public class Index
{
    public string IndexName { get; set; }
    public int ReplicaCount { get; set; }
    public int ShardNumber { get; set; }
    public string AliasName { get; set; }
}