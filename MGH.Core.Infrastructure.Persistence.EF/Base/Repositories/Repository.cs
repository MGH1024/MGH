using MGH.Core.Domain.Base;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.EF.Extensions;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

namespace MGH.Core.Infrastructure.Persistence.EF.Base.Repository;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
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

    public async Task<TEntity> GetAsync(GetModel<TEntity> getBaseModel, CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!getBaseModel.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (getBaseModel.Include != null)
            queryable = getBaseModel.Include(queryable);
        if (getBaseModel.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(getBaseModel.Predicate, cancellationToken);
    }

    public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    public async Task<IPaginate<TEntity>> GetListAsync(GetListModelAsync<TEntity> getListAsyncModel, CancellationToken cancellationToken = default)
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
                .ToPaginateAsync(getListAsyncModel.Index, getListAsyncModel.Size, from: 0, cancellationToken);
        return await queryable.ToPaginateAsync(getListAsyncModel.Index, getListAsyncModel.Size, from: 0, cancellationToken);
    }

    public async Task<IPaginate<TEntity>> GetDynamicListAsync(GetDynamicListModelAsync<TEntity> dynamicListAsyncModel, CancellationToken cancellationToken = default)
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
        return await queryable.ToPaginateAsync(dynamicListAsyncModel.Index, dynamicListAsyncModel.Size, from: 0, cancellationToken);
    }

    public async Task<bool> AnyAsync(GetBaseModel<TEntity> getBaseModel, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (getBaseModel.EnableTracking)
            queryable = queryable.AsNoTracking();
        if (getBaseModel.WithDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (getBaseModel.Predicate != null)
            queryable = queryable.Where(getBaseModel.Predicate);
        return await queryable.AnyAsync(cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(entity, cancellationToken);
        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(entity);
        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        _dbContext.Remove(entity);
        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken);
    }
}