using MGH.Core.Domain.BaseEntity.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MGH.Core.Infrastructure.Cache.Redis.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MGH.Core.Infrastructure.Persistence.EF.Interceptors;

public class RemoveCacheInterceptor(ICachingService<IEntityType> cachingService) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var modifiedEntries = dbContext.ChangeTracker.Entries<IAuditAbleEntity>().ToList();
        foreach (var item in modifiedEntries)
        {
            var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
            if (entityType is null)
                continue;

            if (item.State is EntityState.Modified or EntityState.Deleted)
            {
                var specificKey = GenerateSpecificKeyForEntities(entityType, item);
                cachingService.Remove(specificKey);
                var patternKey = GeneratePatternKeyForEntities(entityType);
                cachingService.RemoveByPattern(patternKey);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    private static string GeneratePatternKeyForEntities(IEntityType entityType)
    {
        return entityType?.ClrType.Name is { } entityName ? $"GetList_{entityName}" : string.Empty;
    }

    private static string GenerateSpecificKeyForEntities(IEntityType entityType, EntityEntry<IAuditAbleEntity> item)
    {
        if (entityType?.ClrType.Name is null)
            return string.Empty;

        var entityName = entityType.ClrType.Name;
        var primaryKey = entityType.FindPrimaryKey();

        if (primaryKey == null)
            return entityName;

        var idValue = GetPkValue(item, primaryKey);
        return $"{entityName}_Id:{idValue}";
    }

    private static object GetPkValue(EntityEntry<IAuditAbleEntity> item, IKey primaryKey)
    {
        var keyValues = primaryKey.Properties
            .Select(p => item.Property(p.Name).CurrentValue)
            .ToArray();
        return keyValues.Length == 1 ? keyValues[0] : string.Join("_", keyValues);
    }
}