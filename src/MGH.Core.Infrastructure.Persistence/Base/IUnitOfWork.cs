namespace MGH.Core.Infrastructure.Persistence.Base;

public interface IUnitOfWork
{
    Task<int> CompleteAsync(CancellationToken cancellationToken);
}