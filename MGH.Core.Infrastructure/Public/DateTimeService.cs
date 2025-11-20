namespace MGH.Core.Infrastructure.Public;

public class DateTimeService : IDateTime
{
    private static readonly TimeZoneInfo IranTimeZone =
         TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");

    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime IranNow =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IranTimeZone);
}