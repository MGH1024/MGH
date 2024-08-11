namespace MGH.Core.Persistence.Models.Filters.GetModels;

public class GetDynamicListAsyncModel<TEntity> :GetModel<TEntity>
{
    public int Index { get; set; } = 0;
    public int Size { get; set; } = 10;
    public DynamicQuery Dynamic { get; set; }
}