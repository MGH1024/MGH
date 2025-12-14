using MGH.Core.Infrastructure.Persistence.Paging;
using MGH.Core.Infrastructure.Persistence.Specifications;

namespace MGH.Core.Infrastructure.Persistence.Base;

/// <summary>
/// Generic repository interface for basic CRUD and query operations using specifications.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public interface IRepository<TEntity, in TKey>
{
    /// <summary>
    /// Gets a single entity that satisfies the given specification.
    /// </summary>
    /// <param name="specification">The specification to filter the entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<TEntity?> GetAsync(
        Specification<TEntity> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a single entity by its primary key.
    /// </summary>
    /// <param name="id">The entity's primary key.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<TEntity?> GetAsync(
        TKey id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paged result of entities that satisfy the given specification.
    /// </summary>
    /// <param name="specification">The specification to filter the entities.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IPagedResult<TEntity>> GetListAsync(
        PagedSpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paged result of entities using a dynamic specification.
    /// </summary>
    /// <param name="specification">The dynamic specification to filter and sort the entities.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IPagedResult<TEntity>> GetDynamicListAsync(
        DynamicPagedSpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if any entity exists that satisfies the given specification.
    /// </summary>
    /// <param name="specification">The specification to filter entities.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<bool> AnyAsync(
        Specification<TEntity> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="autoSave">Whether to immediately save changes to the database.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<TEntity> AddAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="autoSave">Whether to immediately save changes to the database.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<TEntity> UpdateAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="autoSave">Whether to immediately save changes to the database.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DeleteAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default);
}
