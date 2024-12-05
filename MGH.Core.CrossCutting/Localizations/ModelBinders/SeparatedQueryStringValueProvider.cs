using System.Globalization;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace MGH.Core.CrossCutting.Localizations.ModelBinders;

public class SeparatedQueryStringValueProvider : Microsoft.AspNetCore.Mvc.ModelBinding.QueryStringValueProvider
{
    private readonly string _key;
    private readonly string _separator;
    private readonly IQueryCollection _values;

    public SeparatedQueryStringValueProvider(IQueryCollection values, string separator) : this(null, values, separator)
    {
    }

    public SeparatedQueryStringValueProvider(string key, IQueryCollection values, string separator)
        : base(BindingSource.Query, values, CultureInfo.InvariantCulture)

    {
        _key = key;
        _values = values;
        _separator = separator;

        var sss = HttpUtility.ParseQueryString(values.ToString());
    }

    public override ValueProviderResult GetValue(string key)
    {
        var result = base.GetValue(key);

        if (CheckExistLoadOption(key))
            return result;

        if (_key != null && _key != key)
        {
            return result;
        }

        if (result == ValueProviderResult.None ||
            !result.Values.Any(x => x.IndexOf(_separator, StringComparison.OrdinalIgnoreCase) > 0)) return result;


        if (result.Values.ToString().StartsWith("[") && result.Values.ToString().EndsWith("]"))
        {
            return new ValueProviderResult(JsonSerializer.Deserialize<string[]>(result.Values));
        }

        var splitValues = new StringValues(result.Values
            .SelectMany(x => x.Split(new[] { _separator }, StringSplitOptions.None)).ToArray());
        return new ValueProviderResult(splitValues, result.Culture);
    }

    private bool CheckExistLoadOption(string key)
    {
        var list = new List<string>()
        {
            "requireTotalCount", "isCountQuery", "skip", "take", "sort", "group", "filter", "totalSummary",
            "groupSummary", "select"
        };

        return list.Contains(key, StringComparer.OrdinalIgnoreCase);
    }
}