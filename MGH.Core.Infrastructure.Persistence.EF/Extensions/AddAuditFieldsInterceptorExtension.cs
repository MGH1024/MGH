using System.Text.Json;
using MGH.Core.Domain.BaseEntity.Abstract;
using MGH.Core.Domain.Entity.Outboxes;
using MGH.Core.Infrastructure.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MGH.Core.Infrastructure.Persistence.EF.Extensions;

public static class AddAuditFieldsInterceptorExtension
{
    public static void SetOutbox(this DbContextEventData eventData, DbContext dbContext)
    {
        var outboxMessages =
            eventData.Context?.ChangeTracker.Entries<IAggregateRoot>()
                .Select(a => a.Entity)
                .Where(a => a.Events.Any())
                .SelectMany(a => a.Events)
                .Select(a => new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Type = a.GetType().Name,
                    Content = JsonSerializer.Serialize(a, new JsonSerializerOptions
                    {
                        WriteIndented = false,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    })
                }).ToList();

        if (outboxMessages != null)
            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }
    
    public static void SetAuditEntries(this DbContextEventData eventData,AuditInterceptorDto auditInterceptorDto)
    {
        var modifiedEntries = eventData.Context?.ChangeTracker.Entries<IAuditAbleEntity>().ToList();
        if (modifiedEntries == null) return;
        foreach (var item in modifiedEntries)
        {
            var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
            if (entityType is null)
                continue;

            if (item.State == EntityState.Added)
                item.AttachAddedState(auditInterceptorDto);
            
            if (item.State == EntityState.Deleted)
                item.AttachDeletedState(auditInterceptorDto);
            
            if (item.State == EntityState.Modified)
            {
                item.AttachModifiedState(auditInterceptorDto);
                item.AttachDeletedState(auditInterceptorDto);
            }
        }
    }
    
    private static void AttachAddedState(this EntityEntry item,AuditInterceptorDto auditInterceptorDto)
    {
        item.Property("CreatedAt").CurrentValue = auditInterceptorDto.Now;
        item.Property("CreatedBy").CurrentValue = auditInterceptorDto.Username;
        item.Property("CreatedByIp").CurrentValue = auditInterceptorDto.IpAddress;
    }

    private static void AttachDeletedState(this EntityEntry item, AuditInterceptorDto auditInterceptorDto)
    {
        var deletedAtValue = item.Property("DeletedAt").CurrentValue;
        if (deletedAtValue is not null)
        {
            item.Property("DeletedAt").CurrentValue =  auditInterceptorDto.Now;
            item.Property("DeletedBy").CurrentValue = auditInterceptorDto.Username;
            item.Property("DeletedByIp").CurrentValue = auditInterceptorDto.IpAddress;
        }
    }

    private static void AttachModifiedState(this EntityEntry item, AuditInterceptorDto auditInterceptorDto)
    {
        item.Property("UpdatedAt").CurrentValue =  auditInterceptorDto.Now;
        item.Property("UpdatedBy").CurrentValue = auditInterceptorDto.Username;
        item.Property("UpdatedByIp").CurrentValue = auditInterceptorDto.IpAddress;
    }
}