namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public class ElasticSearchConfig(string connectionString, string userName, string password, IEnumerable<Index> indices)
{
    public string ConnectionString { get; set; } = connectionString;
    public string UserName { get; set; } = userName;
    public string Password { get; set; } = password;
    public IEnumerable<Index> Indices { get; set; } = indices;

    public ElasticSearchConfig() : this(string.Empty, string.Empty, string.Empty, [])
    {
    }
}