using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MGH.Core.CrossCutting.Localizations.ModelBinders;
public class UtcAwareDateTimeModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var modelState = bindingContext.ModelState;
        modelState.SetModelValue(modelName, valueProviderResult);
        var metadata = bindingContext.ModelMetadata;
        var type = metadata.UnderlyingOrModelType;
        var value = valueProviderResult.FirstValue;
        object model;
        if (string.IsNullOrEmpty(value))
        {
            model = null;
        }
        else if (type == typeof(DateTime))
        {
            model = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }
        else
        {
            throw new NotSupportedException();
        }

        if (model == null && !metadata.IsReferenceOrNullableType)
        {
            modelState.TryAddModelError(
                modelName,
                metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
                    valueProviderResult.ToString()));
        }
        else
        {
            bindingContext.Result = ModelBindingResult.Success(model);
        }
        return Task.CompletedTask;
    }
}