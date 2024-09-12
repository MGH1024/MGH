﻿using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace MGH.Core.Persistence.Base.Repository;

public interface IAggregateRepository<TEntity, in TKey>
{
    Task<TEntity> GetAsync(GetModel<TEntity> getBaseModel);
    Task<TEntity> GetAsync(TKey id,CancellationToken cancellationToken);
    Task<IPaginate<TEntity>> GetListAsync(GetListModelAsync<TEntity> getListAsyncModel);
    Task<IPaginate<TEntity>> GetDynamicListAsync(GetDynamicListModelAsync<TEntity> dynamicListAsyncModel);
    Task<bool> AnyAsync(GetBaseModel<TEntity> getBaseModel, CancellationToken cancellationToken);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);
}