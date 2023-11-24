using MediatR;

namespace MGH.Core.Application.Buses.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}