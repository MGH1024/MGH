namespace MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;

public class SearchByFieldParameters(string fieldName, string value) : SearchParameters
{
    public string FieldName { get; set; } = fieldName;
    public string Value { get; set; } = value;

    public SearchByFieldParameters() : this(string.Empty, string.Empty)
    {
    }
}
