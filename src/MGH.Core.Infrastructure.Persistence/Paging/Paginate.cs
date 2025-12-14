using System;
using System.Collections.Generic;
using System.Linq;

namespace MGH.Core.Infrastructure.Persistence.Paging;

/// <summary>
/// Represents a paged result in the domain.
/// </summary>
/// <typeparam name="T">Type of items in the page.</typeparam>
public class Paginate<T> : IPagedResult<T>
{
    public Paginate(IEnumerable<T> items, int pageIndex, int pageSize, int totalCount)
    {
        if (pageIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        if (totalCount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalCount));

        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;

        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        Items = items?.ToList() ?? new List<T>();
    }

    public int PageIndex { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages { get; }

    public IList<T> Items { get; }

    public bool HasPrevious => PageIndex > 0;

    public bool HasNext => PageIndex + 1 < TotalPages;
}

/// <summary>
/// Converts a paged result of one type to another type.
/// </summary>
public class Paginate<TSource, TResult> : IPagedResult<TResult>
{
    public Paginate(IPagedResult<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        PageIndex = source.PageIndex;
        PageSize = source.PageSize;
        TotalCount = source.TotalCount;
        TotalPages = source.TotalPages;
        Items = new List<TResult>(converter(source.Items));
    }

    public int PageIndex { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages { get; }

    public IList<TResult> Items { get; }

    public bool HasPrevious => PageIndex > 0;

    public bool HasNext => PageIndex + 1 < TotalPages;
}

/// <summary>
/// Helper factory for creating empty or converted paged results.
/// </summary>
public static class Paginate
{
    public static IPagedResult<T> Empty<T>() => new Paginate<T>(Array.Empty<T>(), 0, 10, 0);

    public static IPagedResult<TResult> From<TResult, TSource>(
        IPagedResult<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter
    ) => new Paginate<TSource, TResult>(source, converter);
}
