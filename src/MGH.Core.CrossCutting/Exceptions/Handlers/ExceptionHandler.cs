using MGH.Core.CrossCutting.Exceptions.Types;

namespace MGH.Core.CrossCutting.Exceptions.Handlers;

public abstract class ExceptionHandler
{
    public Task HandleExceptionAsync(Exception exception) =>
        exception switch
        {
            BusinessException businessException => HandleException(businessException),
            ValidationException validationException => HandleException(validationException),
            AuthorizationException authorizationException => HandleException(authorizationException),
            NotFoundException notFoundException => HandleException(notFoundException),
            BadRequestException badRequestException => HandleException(badRequestException),
            _ => HandleException(exception)
        };

    protected abstract Task HandleException(BusinessException businessException);
    protected abstract Task HandleException(ValidationException validationException);
    protected abstract Task HandleException(AuthorizationException authorizationException);
    protected abstract Task HandleException(NotFoundException notFoundException);
    protected abstract Task HandleException(BadRequestException badRequestException);
    protected abstract Task HandleException(Exception exception);
}
