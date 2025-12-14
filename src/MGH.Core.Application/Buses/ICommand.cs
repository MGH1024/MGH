using MediatR;

namespace MGH.Core.Application.Buses;
public interface ICommand : IRequest
{
}
public interface ICommand<out TResult> : IRequest<TResult>
{
}


