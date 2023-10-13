using MGH.Domain;
using System.Reflection;
using MGH.Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MGH.EntityFramework.Concrete;

public abstract class DbContextBase : DbContext
{
    protected DbContextBase()
    {
    }

    protected DbContextBase(DbContextOptions options) : base(options)
    {

    }
    private IDbContextTransaction _currentTransaction;

    protected abstract Assembly ConfigurationsAssembly { get; }
    public bool HasActiveTransaction => _currentTransaction != null;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(ConfigurationsAssembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x is { Entity: IAuditable, State: EntityState.Modified });

        foreach (var entity in entities)
        {
            var modifiedProperty = entity.Entity.GetType().GetProperty(nameof(IAuditable.UpdatedAt));
            modifiedProperty?.SetValue(entity.Entity, DateTime.Now);
        }
    }

    public IDbContextTransaction GetCurrentTransaction()
    {
        return _currentTransaction;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction != null) return null;
        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
        return _currentTransaction;
    }

    public IDbContextTransaction BeginTransaction()
    {
        if (_currentTransaction != null) return null;
        _currentTransaction = Database.BeginTransaction();
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        try
        {
            await SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _currentTransaction?.RollbackAsync(cancellationToken)!;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}