using Microsoft.EntityFrameworkCore;
using MGH.Core.Infrastructure.Persistence.Paging;

namespace MGH.Core.Infrastructure.Persistence.EF.Extensions;

public static class QueryablePaginateExtensions
{
    /// <summary>
    /// Converts an IQueryable to a paged result asynchronously.
    /// </summary>
    public static async Task<IPagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        int totalCount = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await source
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new Paginate<T>(items, pageIndex, pageSize, totalCount);
    }

    /// <summary>
    /// Converts an IQueryable to a paged result synchronously.
    /// </summary>
    public static IPagedResult<T> ToPagedResult<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize)
    {
        if (pageIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        int totalCount = source.Count();
        var items = source
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToList();

        return new Paginate<T>(items, pageIndex, pageSize, totalCount);
    }
}
