namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public class IndexModel(string indexName, string aliasName)
{
    public string IndexName { get; set; } = indexName;
    public string AliasName { get; set; } = aliasName;
    public int NumberOfReplicas { get; set; } = 3;
    public int NumberOfShards { get; set; } = 3;

    public IndexModel() : this(string.Empty, string.Empty)
    {
    }
}
