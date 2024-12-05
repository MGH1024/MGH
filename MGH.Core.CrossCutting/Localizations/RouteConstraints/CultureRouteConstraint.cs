using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MGH.Core.CrossCutting.Localizations.RouteConstraints;

public class CultureRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        var isMatch = false;

        if (values.ContainsKey("culture"))
        {
            var cultureRouteValue = values["culture"].ToString();

            var isCultureValid = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Any(culture => string.Equals(culture.Name, cultureRouteValue, StringComparison.OrdinalIgnoreCase));

            if (isCultureValid)
            {
                isMatch = true;
            }
        }

        return isMatch;
    }
}