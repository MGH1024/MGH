using System.Text.Json;
using System.Text.Json.Serialization;

namespace MGH.Core.Infrastructure.Caching
{
    internal static class JsonHelper
    {
        internal static T? Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }

        internal static string Serialize<T>(T value)
        {
            if (value == null)
                return string.Empty;

            return JsonSerializer.Serialize(value, _jsonOptions);
        }

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter(),
            }
        };
    }
}
