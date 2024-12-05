namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public class SearchByQueryParameters(string queryName, string query, string[] fields) : SearchParameters
{
    public string QueryName { get; set; } = queryName;
    public string Query { get; set; } = query;
    public string[] Fields { get; set; } = fields;

    public SearchByQueryParameters() : this(string.Empty, string.Empty, Array.Empty<string>())
    {
    }
}
