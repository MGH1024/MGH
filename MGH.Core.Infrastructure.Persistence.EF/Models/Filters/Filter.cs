namespace MGH.Core.Infrastructure.Persistence.EF.Models.Filters;

public class Filter(string field, string @operator, string value, string logic, IEnumerable<Filter> filters)
{
    public string Field { get; set; } = field;
    public string Operator { get; set; } = @operator;
    public string Value { get; set; } = value;
    public string Logic { get; set; } = logic;
    public IEnumerable<Filter> Filters { get; set; } = filters;
}
