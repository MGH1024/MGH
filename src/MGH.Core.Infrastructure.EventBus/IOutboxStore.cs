using MGH.Core.Infrastructure.Persistence.Entities;

namespace MGH.Core.Infrastructure.EventBus
{
    public interface IOutboxStore
    {
        Task AddToOutBoxAsync(OutboxMessage message, CancellationToken cancellationToken = default);
        Task AddToOutBoxRangeAsync(IEnumerable<OutboxMessage> messages, CancellationToken cancellationToken = default);
    }
}
