using MGH.Core.Infrastructure.Public;
using MGH.Core.Persistence.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MGH.Core.Persistence.Interceptors;

public class AddAuditFieldsInterceptor(IDateTime dateTime) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var now = dateTime.IranNow;
        var userName = "admin";
        var dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        if (eventData.Context == null) 
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        eventData.SetAuditEntries( now, userName);
        eventData.SetOutbox( dbContext);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    
}