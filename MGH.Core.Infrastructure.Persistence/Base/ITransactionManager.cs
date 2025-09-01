using Microsoft.EntityFrameworkCore;

namespace MGH.Core.Infrastructure.Persistence.Base;

public interface ITransactionManager<TContext> where TContext : DbContext
{
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}
