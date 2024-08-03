using MediatR;

namespace MGH.Core.Domain.Buses.Commands;
public interface ICommand : IRequest
{
}
public interface ICommand<out TResult> : IRequest<TResult>
{
}


