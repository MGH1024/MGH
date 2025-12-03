using Microsoft.AspNetCore.Http;
using MGH.Core.Infrastructure.Public;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MGH.Core.Infrastructure.Persistence.EF.Extensions;

namespace MGH.Core.Infrastructure.Persistence.EF.Interceptors;

public class AuditFieldsInterceptor(IDateTime dateTime, IHttpContextAccessor httpContextAccessor)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var auditInterceptorDto = new AuditInterceptorDto(GetCurrentUsername(), GetCurrentIpAddress(), dateTime.UtcNow);

        if (eventData.Context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        eventData.SetAuditEntries(auditInterceptorDto);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private string GetCurrentUsername()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
    }

    private string GetCurrentIpAddress()
    {
        return httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString() ?? string.Empty;
    }
}