using MediatR;

namespace MGH.Core.Application.Buses;
public interface IQuery<out TResult> : IRequest<TResult>
{
}
