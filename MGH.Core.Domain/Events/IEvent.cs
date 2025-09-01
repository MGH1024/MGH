namespace MGH.Core.Domain.Events;

public interface IEvent
{
     Guid Id { get; set; }
     DateTime OccurredOn { get; set; }
}