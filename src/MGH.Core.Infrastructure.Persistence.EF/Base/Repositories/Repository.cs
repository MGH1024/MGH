using MGH.Core.Domain.Base;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Persistence.Base;
using MGH.Core.Infrastructure.Persistence.Paging;
using MGH.Core.Infrastructure.Persistence.EF.Extensions;
using MGH.Core.Infrastructure.Persistence.Specifications;

namespace MGH.Core.Infrastructure.Persistence.EF.Base.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    private readonly DbContext _dbContext;

    protected Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected IQueryable<TEntity> Query() => _dbContext.Set<TEntity>();

    // =========================
    // Get single entity by specification
    // =========================
    public async Task<TEntity?> GetAsync(
        Specification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var queryable = ApplySpecification(Query(), specification);
        return await queryable.FirstOrDefaultAsync(cancellationToken);
    }

    // =========================
    // Get by Id
    // =========================
    public async Task<TEntity?> GetAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    // =========================
    // Get paged list
    // =========================
    public async Task<IPagedResult<TEntity>> GetListAsync(
        PagedSpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var queryable = ApplySpecification(Query(), specification);

        if (specification.OrderBy != null)
        {
            queryable = specification.OrderByDescending
                ? queryable.OrderByDescending(specification.OrderBy)
                : queryable.OrderBy(specification.OrderBy);
        }

        var totalCount = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .Skip(specification.PageIndex * specification.PageSize)
            .Take(specification.PageSize)
            .ToListAsync(cancellationToken);

        return new Paginate<TEntity>(items, specification.PageIndex, specification.PageSize, totalCount);
    }

    // =========================
    // Get dynamic paged list
    // =========================
    public async Task<IPagedResult<TEntity>> GetDynamicListAsync(
        DynamicPagedSpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var queryable = ApplySpecification(Query().ToDynamic(specification.Dynamic), specification);

        var totalCount = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .Skip(specification.PageIndex * specification.PageSize)
            .Take(specification.PageSize)
            .ToListAsync(cancellationToken);

        return new Paginate<TEntity>(items, specification.PageIndex, specification.PageSize, totalCount);
    }

    // =========================
    // AnyAsync
    // =========================
    public async Task<bool> AnyAsync(
        Specification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var queryable = ApplySpecification(Query(), specification);
        return await queryable.AnyAsync(cancellationToken);
    }

    // =========================
    // Add
    // =========================
    public async Task<TEntity> AddAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(entity, cancellationToken);

        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    // =========================
    // Update
    // =========================
    public async Task<TEntity> UpdateAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Update(entity);

        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    // =========================
    // Delete
    // =========================
    public async Task DeleteAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Remove(entity);

        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken);
    }

    // =========================
    // Apply specification helper
    // =========================
    private static IQueryable<TEntity> ApplySpecification(
        IQueryable<TEntity> queryable,
        Specification<TEntity> specification)
    {
        if (!specification.EnableTracking)
            queryable = queryable.AsNoTracking();

        if (specification.IncludeSoftDeleted)
            queryable = queryable.IgnoreQueryFilters();

        if (specification.Criteria != null)
            queryable = queryable.Where(specification.Criteria);

        foreach (var include in specification.Includes)
            queryable = queryable.Include(include);

        return queryable;
    }
}
