namespace MGH.Core.Domain.BaseEntity.Abstract.Features;

public interface IPageable
{
    int Row { get; }
    int TotalCount { get; }
    int CurrentPage { get; }
    int PageSize { get; }
}