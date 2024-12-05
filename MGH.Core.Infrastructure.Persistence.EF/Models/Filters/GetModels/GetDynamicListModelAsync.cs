namespace MGH.Core.Infrastructure.Persistence.EF.Models.Filters.GetModels;

public class GetDynamicListModelAsync<TEntity> :GetModel<TEntity>
{
    public int Index { get; set; } = 0;
    public int Size { get; set; } = 10;
    public DynamicQuery Dynamic { get; set; }
}