using Microsoft.EntityFrameworkCore.Query;

namespace MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;

public class GetModel<TEntity> : GetBaseModel<TEntity>
{
    public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> Include { get; set; } = null;
}