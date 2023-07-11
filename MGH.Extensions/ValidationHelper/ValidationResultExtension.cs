using MGH.Exceptions;
using MGH.Exceptions.Models;
using FluentValidation.Results;

namespace MGH.Extensions.ValidationHelper;

public static class ValidationResultExtension
{
    public static void RaiseExceptionIfRequired(this ValidationResult validationResult)
    {
        if (validationResult.IsValid)
            return;

        var validationErrors =
            validationResult
                .Errors
                .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage))
                .ToList();

        throw new CustomValidationException(validationErrors);
    }
}