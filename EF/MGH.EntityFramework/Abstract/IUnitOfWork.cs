using Microsoft.EntityFrameworkCore.Storage;

namespace MGH.EntityFramework.Abstract;

public interface IUnitOfWork : IDisposable
{
    bool TransactionEnabled { get; }
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    IDbContextTransaction BeginTransaction();
    Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}