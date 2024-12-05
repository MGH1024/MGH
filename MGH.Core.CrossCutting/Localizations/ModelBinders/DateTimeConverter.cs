using System.Text.Json;
using System.Text.Json.Serialization;

namespace MGH.Core.CrossCutting.Localizations.ModelBinders;

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDateTime().ToLocalTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var un = value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        writer.WriteStringValue(un);
    }
}
