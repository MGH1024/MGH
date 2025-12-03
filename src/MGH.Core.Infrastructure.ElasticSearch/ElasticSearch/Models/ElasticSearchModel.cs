using Nest;

namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public class ElasticSearchModel(Id elasticId, string indexName)
{
    public Id ElasticId { get; set; } = elasticId;
    public string IndexName { get; set; } = indexName;

    public ElasticSearchModel() : this(null!, string.Empty)
    {
    }
}
