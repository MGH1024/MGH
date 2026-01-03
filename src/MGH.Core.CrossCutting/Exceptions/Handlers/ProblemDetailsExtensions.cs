using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MGH.Core.CrossCutting.Exceptions.Handlers;

public static class ProblemDetailsExtensions
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public static string AsJson(this ProblemDetails details)
    {
        return JsonSerializer.Serialize(details, DefaultOptions);
    }
}
