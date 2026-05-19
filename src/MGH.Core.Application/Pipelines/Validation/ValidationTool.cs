using FluentValidation;

namespace MGH.Core.Application.Pipelines.Validation;

public static class ValidationTool
{
    public static void Validate(IValidator validator, object entity)
    {
        ValidationContext<object> context = new(entity);
        var result = validator.Validate(context);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}
