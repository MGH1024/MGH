namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public interface IElasticSearchResult
{
    public bool Success { get; }
    public string Message { get; }
}
