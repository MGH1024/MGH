using MediatR;

namespace MGH.Core.Domain.Buses.Queries;
public interface IQuery<out TResult> : IRequest<TResult>
{
}
