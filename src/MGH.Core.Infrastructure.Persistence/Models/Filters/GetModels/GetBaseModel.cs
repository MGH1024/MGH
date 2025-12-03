using System.Linq.Expressions;

namespace MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

public class GetBaseModel<TEntity>
{
    public bool WithDeleted { get; set; } = false;
    public bool EnableTracking { get; set; } = true; 
    public Expression<Func<TEntity, bool>> Predicate { get; set; } = null;
}