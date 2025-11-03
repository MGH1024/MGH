using System.Collections;
using MGH.Core.Domain.Audits;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.EF.Extensions;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MGH.Core.Infrastructure.Persistence.EF.Base.Repository;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IFullAuditable
{
    private readonly DbContext _dbContext;

    protected Repository()
    {

    }

    protected Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private IQueryable<TEntity> Query() => _dbContext.Set<TEntity>();

    public async Task<TEntity> GetAsync(GetModel<TEntity> getBaseModel)
    {
        var queryable = Query();
        if (!getBaseModel.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (getBaseModel.Include != null)
            queryable = getBaseModel.Include(queryable);
        if (getBaseModel.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(getBaseModel.Predicate, getBaseModel.CancellationToken);
    }

    public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    public async Task<IPaginate<TEntity>> GetListAsync(GetListModelAsync<TEntity> getListAsyncModel)
    {
        IQueryable<TEntity> queryable = Query();
        if (!getListAsyncModel.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (getListAsyncModel.Include != null)
            queryable = getListAsyncModel.Include(queryable);
        if (getListAsyncModel.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (getListAsyncModel.Predicate != null)
            queryable = queryable.Where(getListAsyncModel.Predicate);
        if (getListAsyncModel.OrderBy != null)
            return await getListAsyncModel.OrderBy(queryable)
                .ToPaginateAsync(getListAsyncModel.Index, getListAsyncModel.Size, from: 0, getListAsyncModel.CancellationToken);
        return await queryable.ToPaginateAsync(getListAsyncModel.Index, getListAsyncModel.Size, from: 0, getListAsyncModel.CancellationToken);
    }

    public async Task<IPaginate<TEntity>> GetDynamicListAsync(GetDynamicListModelAsync<TEntity> dynamicListAsyncModel)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamicListAsyncModel.Dynamic);
        if (!dynamicListAsyncModel.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (dynamicListAsyncModel.Include != null)
            queryable = dynamicListAsyncModel.Include(queryable);
        if (dynamicListAsyncModel.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (dynamicListAsyncModel.Predicate != null)
            queryable = queryable.Where(dynamicListAsyncModel.Predicate);
        return await queryable.ToPaginateAsync(dynamicListAsyncModel.Index, dynamicListAsyncModel.Size, from: 0,
            dynamicListAsyncModel.CancellationToken);
    }

    public async Task<bool> AnyAsync(GetBaseModel<TEntity> getBaseModel, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> queryable = Query();
        if (getBaseModel.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (getBaseModel.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (getBaseModel.Predicate != null)
            queryable = queryable.Where(getBaseModel.Predicate);
        return await queryable.AnyAsync(getBaseModel.CancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(entity, cancellationToken);
        return entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
    {
        await SetEntityAsDeletedAsync(entity, permanent);
        return entity;
    }

    private async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            await SetEntityAsSoftDeletedAsync(entity);
        }
        else
        {
            _dbContext.Remove(entity);
        }
    }

    private void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
        bool hasEntityHaveOneToOneRelation =
            _dbContext
                .Entry(entity)
                .Metadata.GetForeignKeys()
                .All(
                    x =>
                        x.DependentToPrincipal?.IsCollection == true
                        || x.PrincipalToDependent?.IsCollection == true
                        || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
                ) == false;
        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException(
                "Entity has one-to-one relationship. Soft Delete causes problems" +
                " if you try to create entry again by same foreign key."
            );
    }

    private async Task SetEntityAsSoftDeletedAsync(TEntity entity)
    {
        if (entity.DeletedAt.HasValue)
            return;
        entity.DeletedAt = DateTime.UtcNow;

        var navigations = _dbContext
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x => x is
            {
                IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade
            })
            .ToList();
        foreach (INavigation navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            object navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    IQueryable query = _dbContext.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query,
                        navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();
                }

                foreach (TEntity navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeletedAsync(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = _dbContext.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query,
                            navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await SetEntityAsSoftDeletedAsync((TEntity)navValue);
            }
        }

        _dbContext.Update(entity);
    }

    private IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        var queryProviderType = query.Provider.GetType();
        var createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                .MakeGenericMethod(navigationPropertyType);
        var queryProviderQuery =
            (IQueryable<object>)createQueryMethod.Invoke(query.Provider,
                parameters: [query.Expression])!;
        return queryProviderQuery.Where(x => !((IFullAuditable)x).DeletedAt.HasValue);
    }
}