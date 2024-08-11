namespace MGH.Core.Persistence.Models.Filters.GetModels;

public class GetListAsyncModel<TEntity> :GetModel<TEntity>
{
    public int Index { get; set; } = 0;
    public int Size { get; set; } = 10;
    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; set; } = null;
}