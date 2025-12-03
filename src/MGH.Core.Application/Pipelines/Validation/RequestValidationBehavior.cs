using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MGH.Core.CrossCutting.Exceptions.Types;
using ValidationException = MGH.Core.CrossCutting.Exceptions.Types.ValidationException;

namespace MGH.Core.Application.Pipelines.Validation;

public class RequestValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken
        cancellationToken)
    {
        ValidationContext<object> context = new(request);
        var errors = validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .GroupBy(
                keySelector: p => p.PropertyName,
                resultSelector: (propertyName, errors) =>
                    new ValidationExceptionModel
                    {
                        Property = propertyName,
                        Errors = errors.Select(e => e.ErrorMessage)
                    })
            .ToList();

        if (errors.Any())
            throw new ValidationException(errors);
        var response = await next();
        return response;
    }
}