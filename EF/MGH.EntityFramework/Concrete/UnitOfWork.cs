using MGH.EntityFramework.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Storage;

namespace MGH.EntityFramework.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContextBase _context;
    private readonly ILogger<UnitOfWork> _logger;
    public bool TransactionEnabled => _context.HasActiveTransaction;

    public UnitOfWork(IDbContextAccessor dbContextAccessor, ILogger<UnitOfWork> logger)
    {
        _logger = logger;
        _context = dbContextAccessor.Context;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await _context.BeginTransactionAsync(cancellationToken);
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _context.BeginTransaction();
    }

    public async Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        await _context.CommitTransactionAsync(transaction, cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        var transaction = _context.GetCurrentTransaction();
        if (transaction != null)
            await _context.CommitTransactionAsync(transaction, cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await _context.RollbackTransactionAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}