namespace  MGH.Core.Infrastructure.Persistence.Filters;

public class DynamicQuery
{
    public IEnumerable<Sort> Sort { get; set; }
    public Filter Filter { get; set; }

    public DynamicQuery() { }

    public DynamicQuery(IEnumerable<Sort> sort, Filter filter)
    {
        Sort = sort;
        Filter = filter;
    }
}
