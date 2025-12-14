using System.Linq.Expressions;

namespace MGH.Core.Infrastructure.Persistence.Specifications;

public class Specification<TEntity>
{
    /// <summary>
    /// Business rule that entities must satisfy
    /// </summary>
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }

    /// <summary>
    /// Include soft-deleted entities
    /// </summary>
    public bool IncludeSoftDeleted { get; set; } = false;

    /// <summary>
    /// Enable EF Core tracking
    /// </summary>
    public bool EnableTracking { get; set; } = false;

    /// <summary>
    /// Aggregate navigation includes
    /// </summary>
    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();
}