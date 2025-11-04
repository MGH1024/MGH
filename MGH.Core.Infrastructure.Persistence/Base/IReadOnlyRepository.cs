using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.Models.Paging;

namespace MGH.Core.Infrastructure.Persistence.Base;

public interface IReadOnlyRepository<TEntity, in TKey>
{
    Task<TEntity> GetAsync(GetModel<TEntity> getBaseModel, CancellationToken cancellationToken = default);
    Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IPaginate<TEntity>> GetListAsync(GetListModelAsync<TEntity> getListAsyncModel, CancellationToken cancellationToken = default);
    Task<IPaginate<TEntity>> GetDynamicListAsync(GetDynamicListModelAsync<TEntity> dynamicListAsyncModel, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(GetBaseModel<TEntity> getBaseModel, CancellationToken cancellationToken = default);
}