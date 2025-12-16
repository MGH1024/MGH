namespace MGH.Core.Infrastructure.Persistence.Paging;

/// <summary>
/// Represents a paged collection of items in the domain.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public interface IPagedResult<T>
{
    /// <summary>
    /// The zero-based index of the current page.
    /// </summary>
    int PageIndex { get; }

    /// <summary>
    /// The number of items per page.
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// The total number of items across all pages.
    /// </summary>
    int TotalCount { get; }

    /// <summary>
    /// The total number of pages.
    /// </summary>
    int TotalPages { get; }

    /// <summary>
    /// The items in the current page.
    /// </summary>
    IList<T> Items { get; }

    /// <summary>
    /// Indicates if there is a previous page.
    /// </summary>
    bool HasPrevious { get; }

    /// <summary>
    /// Indicates if there is a next page.
    /// </summary>
    bool HasNext { get; }
}
