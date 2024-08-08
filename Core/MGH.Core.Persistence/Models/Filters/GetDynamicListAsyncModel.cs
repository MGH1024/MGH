namespace MGH.Core.Persistence.Models.Filters;

public class GetDynamicListAsyncModel<TEntity> :GetBaseModel<TEntity>
{
    public int Index { get; set; } = 0;
    public int Size { get; set; } = 10;
    public DynamicQuery Dynamic { get; set; }
}