namespace Calendarific.NetClient
{
    public interface ICalendarificConfig
    {
        string BaseUrl { get; }
        string ApiKey { get; }
    }
    public class CalendarificConfig : ICalendarificConfig
    {
        public string BaseUrl { get; set; } = "https://calendarific.com/api/v2/";
        public string ApiKey { get; set; }
    }
}