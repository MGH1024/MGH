using MGH.Core.Infrastructure.Persistence.Entities;

namespace MGH.Core.Infrastructure.Persistence.Base
{
    public interface IOutboxStore
    {
        Task AddToOutBoxAsync(OutboxMessage message, CancellationToken cancellationToken = default);
        Task AddToOutBoxRangeAsync(IEnumerable<OutboxMessage> messages, CancellationToken cancellationToken = default);
    }
}
