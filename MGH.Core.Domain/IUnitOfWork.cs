﻿namespace MGH.Core.Domain;

public interface IUnitOfWork
{
    Task<int> CompleteAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken); 
}