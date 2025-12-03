namespace MGH.Core.Domain.Events;

public interface IEvent
{
     Guid Id { get;  }
     DateTime OccurredOn { get;}
}