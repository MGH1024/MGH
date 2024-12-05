namespace MGH.Core.Infrastructure.Public;

public class DateTimeService : IDateTime
{
    public DateTime IranNow => TimeZoneInfo
        .ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time"));
}