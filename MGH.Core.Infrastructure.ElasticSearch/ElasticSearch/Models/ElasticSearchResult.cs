namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public class ElasticSearchResult(bool success, string message = null) : IElasticSearchResult //todo: refactor
{
    public bool Success { get; } = success;
    public string Message { get; } = message;

    public ElasticSearchResult() : this(false, string.Empty)
    {
    }
}
