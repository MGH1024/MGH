namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public class ElasticSearchGetModel<T>(string elasticId, T item)
{
    public string ElasticId { get; set; } = elasticId;
    public T Item { get; set; } = item;

    public ElasticSearchGetModel() : this(string.Empty, default!)
    {
    }
}
