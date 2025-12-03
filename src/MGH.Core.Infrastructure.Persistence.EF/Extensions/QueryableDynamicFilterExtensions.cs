using System.Linq.Dynamic.Core;
using System.Text;
using MGH.Core.Domain.Base;
using MGH.Core.Infrastructure.Persistence.Models.Filters;

namespace MGH.Core.Infrastructure.Persistence.EF.Extensions;

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

    private static string Transform(Filter filter, IList<Filter> filters, Type type)
    {
        if (string.IsNullOrEmpty(filter.Field))
            throw new ArgumentException("Invalid Field");
        if (string.IsNullOrEmpty(filter.Operator) || !Operators.ContainsKey(filter.Operator))
            throw new ArgumentException("Invalid Operator");
        
        string fieldExpression = GetFieldExpression(type, filter.Field);
        bool isValueObject = CheckIfFieldIsValueObject(type, filter.Field);

        int index = filters.IndexOf(filter);
        string comparison = Operators[filter.Operator];
        StringBuilder where = new();
        
        if (isValueObject)
            throw new InvalidOperationException($"Property '{filter.Field}' is a value object and cannot be queried directly.");
        
        if (!string.IsNullOrEmpty(filter.Value))
        {
            if (filter.Operator == "doesnotcontain")
                where.Append($"(!{fieldExpression}.Contains(@{index}))");
            else if (comparison == "StartsWith")
                where.Append($"({fieldExpression}.StartsWith(@{index}))");
            else if (comparison == "EndsWith")
                where.Append($"({fieldExpression}.EndsWith(@{index}))");
            else if (comparison == "Contains")
                where.Append($"({fieldExpression}.Contains(@{index}))");
            else
                where.Append($"{fieldExpression} {comparison} @{index}");
        }
        else if (filter.Operator == "isnull" || filter.Operator == "isnotnull")
        {
            where.Append($"{fieldExpression} {comparison}");
        }
        
        if (filter.Logic != null && filter.Filters != null && filter.Filters.Any())
        {
            if (!Logics.Contains(filter.Logic))
                throw new ArgumentException("Invalid Logic");

            var nestedFilters = string.Join($" {filter.Logic} ",
                filter.Filters.Select(f => Transform(f, filters, type)).ToArray());
            return $"({where}) {filter.Logic} ({nestedFilters})";
        }

        return where.ToString();
    }

    private static string GetFieldExpression(Type type, string field)
    {
        string[] parts = field.Split('.');
        StringBuilder fieldExpression = new();
        Type currentType = type;

        foreach (var part in parts)
        {
            var property = currentType.GetProperty(part);
            if (property == null)
                throw new ArgumentException($"Invalid Field: {part}");

            string prefix = fieldExpression.Length == 0 ? "" : ".";
            fieldExpression.Append($"{prefix}{part}");
            currentType = property.PropertyType;
        }

        return fieldExpression.ToString();
    }

    private static bool CheckIfFieldIsValueObject(Type type, string field)
    {
        string[] parts = field.Split('.');
        Type currentType = type;

        foreach (var part in parts)
        {
            var property = currentType.GetProperty(part);
            if (property == null)
                throw new ArgumentException($"Invalid Field: {part}");

            if (typeof(ValueObject).IsAssignableFrom(property.PropertyType))
            {
                return true; // This field or property is a value object
            }

            currentType = property.PropertyType;
        }

        return false; // No value object found in the field path
    }
}