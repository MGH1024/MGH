using MGH.Core.Domain.Events;

namespace MGH.Core.Domain.Abstractions.Events;

public interface IHasIntegrationEvent<T>
{
    IEnumerable<IntegrationEvent<T>> GetIntegrationEvents();
    void ClearIntegrationEvents();
}

