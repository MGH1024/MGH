using Microsoft.EntityFrameworkCore.Diagnostics;
using MGH.Core.Infrastructure.Persistence.EF.Extensions;

namespace MGH.Core.Infrastructure.Persistence.EF.Interceptors;

public class OutBoxInterceptor() : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        if (eventData.Context == null) 
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        eventData.SetOutbox(dbContext);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}