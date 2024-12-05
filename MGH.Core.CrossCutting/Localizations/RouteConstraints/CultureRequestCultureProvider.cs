using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace MGH.Core.CrossCutting.Localizations.RouteConstraints;

public class CultureRequestCultureProvider : RequestCultureProvider
{
    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        var culture = httpContext.Request.Path.Value?.Split('/')[1]?.ToString();
        var providerResultCulture = new ProviderCultureResult(culture);

        return Task.FromResult(providerResultCulture);
    }
}