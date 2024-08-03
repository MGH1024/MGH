using System.Linq.Dynamic.Core;
using System.Text;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;

namespace MGH.Core.Infrastructure.Persistence.Persistence.Extensions;

public static class QueryableDynamicFilterExtensions
{
    private static readonly string[] Orders = ["asc", "desc"];
    private static readonly string[] Logics = ["and", "or"];

    private static readonly IDictionary<string, string> Operators = new Dictionary<string, string>
    {
        { "eq", "=" },
        { "neq", "!=" },
        { "lt", "<" },
        { "lte", "<=" },
        { "gt", ">" },
        { "gte", ">=" },
        { "isnull", "== null" },
        { "isnotnull", "!= null" },
        { "startswith", "StartsWith" },
        { "endswith", "EndsWith" },
        { "contains", "Contains" },
        { "doesnotcontain", "Contains" }
    };

    public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, DynamicQuery dynamicQuery)
    {
        if (dynamicQuery.Filter is not null)
            query = Filter(query, dynamicQuery.Filter);
        if (dynamicQuery.Sort is not null && dynamicQuery.Sort.Any())
            query = Sort(query, dynamicQuery.Sort);
        return query;
    }

    private static IQueryable<T> Filter<T>(IQueryable<T> queryable, Filter filter)
    {
        IList<Filter> filters = GetAllFilters(filter);
        string[] values = filters.Select(f => f.Value).ToArray();
        string where = Transform(filter, filters, typeof(T));
        if (!string.IsNullOrEmpty(where))
            queryable = queryable.Where(where, values);

        return queryable;
    }

    private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
    {
        var enumerable = sort as Sort[] ?? sort.ToArray();
        foreach (Sort item in enumerable)
        {
            if (string.IsNullOrEmpty(item.Field))
                throw new ArgumentException("Invalid Field");
            if (string.IsNullOrEmpty(item.Dir) || !Orders.Contains(item.Dir))
                throw new ArgumentException("Invalid Order Type");
        }

        if (enumerable.Any())
        {
            string ordering = string.Join(separator: ",", values: enumerable.Select(s => $"{s.Field} {s.Dir}"));
            return queryable.OrderBy(ordering);
        }

        return queryable;
    }

    private static IList<Filter> GetAllFilters(Filter filter)
    {
        List<Filter> filters = new();
        GetFilters(filter, filters);
        return filters;
    }

    private static void GetFilters(Filter filter, IList<Filter> filters)
    {
        filters.Add(filter);
        if (filter.Filters is not null && filter.Filters.Any())
            foreach (Filter item in filter.Filters)
                GetFilters(item, filters);
    }

    private static string Transform(Filter filter, IList<Filter> filters,Type type)
    {
        var props = type.GetProperties();
        
        if (string.IsNullOrEmpty(filter.Field))
            throw new ArgumentException("Invalid Field");
        if (string.IsNullOrEmpty(filter.Operator) || !Operators.ContainsKey(filter.Operator))
            throw new ArgumentException("Invalid Operator");
        
        int index = filters.IndexOf(filter);
        string comparison = Operators[filter.Operator];
        StringBuilder where = new();

        if (!string.IsNullOrEmpty(filter.Value))
        {
            if (filter.Operator == "doesnotcontain")
                where.Append($"(!np({filter.Field}).{comparison}(@{index.ToString()}))");
            else if (comparison is "StartsWith" or "EndsWith" or "Contains")
                where.Append($"(np({filter.Field}).{comparison}(@{index.ToString()}))");
            else
                where.Append($"np({filter.Field}) {comparison} @{index.ToString()}");
        }
        else if (filter.Operator is "isnull" or "isnotnull")
        {
            where.Append($"np({filter.Field}) {comparison}");
        }

        if (filter.Logic is not null && filter.Filters is not null && filter.Filters.Any())
        {
            if (!Logics.Contains(filter.Logic))
                throw new ArgumentException("Invalid Logic");
            return $"{where} {filter.Logic} ({string.Join(separator: $" {filter.Logic} ", value: filter.Filters.Select(f => Transform(f, filters,type)).ToArray())})";
        }

        return where.ToString();
    }
}
