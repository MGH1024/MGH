namespace MGH.Core.Application.Requests;

public interface IPagedResult
{
    int StartRow { get; }
    int TotalCount { get; }
    int CurrentPage { get; }
    int PageSize { get; }
}