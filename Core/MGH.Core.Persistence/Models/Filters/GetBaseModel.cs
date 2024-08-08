using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace MGH.Core.Persistence.Models.Filters;

public class GetBaseModel<TEntity>
{
    public bool WithDeleted { get; set; } = false;
    public bool EnableTracking { get; set; } = true; 
    public CancellationToken CancellationToken { get; set; } = default;
    public Expression<Func<TEntity, bool>> Predicate { get; set; } = null;
    public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> Include { get; set; } = null;
}