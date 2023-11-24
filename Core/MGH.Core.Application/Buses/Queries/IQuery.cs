using MediatR;

namespace MGH.Core.Application.Buses.Queries;
public interface IQuery<out TResult> : IRequest<TResult>
{
}
