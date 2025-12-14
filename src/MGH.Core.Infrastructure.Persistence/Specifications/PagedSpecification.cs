using System.Linq.Expressions;

namespace MGH.Core.Infrastructure.Persistence.Specifications;

public class PagedSpecification<TEntity> : Specification<TEntity>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;

    public Expression<Func<TEntity, object>>? OrderBy { get; set; }
    public bool OrderByDescending { get; set; }
}