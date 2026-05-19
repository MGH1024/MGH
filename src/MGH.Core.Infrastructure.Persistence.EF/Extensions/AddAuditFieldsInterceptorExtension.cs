using MGH.Core.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MGH.Core.Infrastructure.Persistence.EF.Extensions;

public static class AddAuditFieldsInterceptorExtension
{
    public static void SetAuditEntries(this DbContextEventData eventData, AuditInterceptorDto auditInterceptorDto)
    {
        var modifiedEntries = eventData.Context?.ChangeTracker.Entries<IEntity>().ToList();
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

    private static void AttachAddedState(this EntityEntry item, AuditInterceptorDto auditInterceptorDto)
    {
        item.Property("Created").CurrentValue = auditInterceptorDto.Now;
        item.Property("CreatedBy").CurrentValue = auditInterceptorDto.Username;
        item.Property("CreatedFromIp").CurrentValue = auditInterceptorDto.IpAddress;
    }

    private static void AttachDeletedState(this EntityEntry item, AuditInterceptorDto auditInterceptorDto)
    {
        var deletedAtValue = item.Property("DeletedAt").CurrentValue;
        if (deletedAtValue is not null)
        {
            item.Property("Deleted").CurrentValue = auditInterceptorDto.Now;
            item.Property("DeletedBy").CurrentValue = auditInterceptorDto.Username;
            item.Property("DeletedFromIp").CurrentValue = auditInterceptorDto.IpAddress;
        }
    }

    private static void AttachModifiedState(this EntityEntry item, AuditInterceptorDto auditInterceptorDto)
    {
        item.Property("Modified").CurrentValue = auditInterceptorDto.Now;
        item.Property("ModifiedBy").CurrentValue = auditInterceptorDto.Username;
        item.Property("ModifiedFromIp").CurrentValue = auditInterceptorDto.IpAddress;
    }
}