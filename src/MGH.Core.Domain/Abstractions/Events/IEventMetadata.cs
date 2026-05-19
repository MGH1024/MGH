namespace MGH.Core.Domain.Abstractions.Events;

public interface IEventMetadata
{
     Guid Id { get;  }
     DateTime OccurredOn { get;}
}