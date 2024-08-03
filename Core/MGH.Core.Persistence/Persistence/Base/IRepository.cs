﻿using System.Linq.Expressions;
using MGH.Core.Domain.Entity.Base;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore.Query;

namespace MGH.Core.Infrastructure.Persistence.Persistence.Base;

public interface IRepository<TEntity, TEntityId> : IQuery<TEntity>
    where TEntity : AuditAbleEntity<TEntityId>
{
    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<IPaginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<IPaginate<TEntity>> GetDynamicListAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    
    Task<TEntity> AddAsync(TEntity entity);
    
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entity);
    
    Task<TEntity> UpdateAsync(TEntity entity);
    
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entity);
    
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);
    
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entity, bool permanent = false);
}