using System.Text.Json;
using MGH.Core.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MGH.Core.Infrastructure.Persistence.Entities;

namespace MGH.Core.Infrastructure.Persistence.EF.Interceptors;

public class OutboxEntityInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var aggregates = context.ChangeTracker
            .Entries<AggregateRoot<Guid>>() 
            .Select(e => e.Entity)
            .Where(a => a.DomainEvents.Any())
            .ToList();

        if (!aggregates.Any())
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = new List<OutboxMessage>();

        foreach (var aggregate in aggregates)
        {
            foreach (var domainEvent in aggregate.DomainEvents)
            {
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    Type = domainEvent.GetType().FullName!,
                    Payload = JsonSerializer.Serialize(domainEvent),
                    OccurredOn = DateTime.UtcNow
                };

                outboxMessages.Add(outboxMessage);
            }

            aggregate.ClearDomainEvents();
        }

        context.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
