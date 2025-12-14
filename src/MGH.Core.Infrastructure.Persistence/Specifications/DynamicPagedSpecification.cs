using MGH.Core.Infrastructure.Persistence.Filters;

namespace MGH.Core.Infrastructure.Persistence.Specifications;

public class DynamicPagedSpecification<TEntity> : Specification<TEntity>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;

    public DynamicQuery? Dynamic { get; set; }
}