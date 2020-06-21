using Newtonsoft.Json;

namespace Calendarific.NetClient
{
    public class Datetime
    {

        [JsonProperty("year", Required = Required.Always)]
        public int Year { get; set; }

        [JsonProperty("month", Required = Required.Always)]
        public int Month { get; set; }

        [JsonProperty("day", Required = Required.Always)]
        public int Day { get; set; }

        [JsonProperty("hour", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Hour { get; set; }

        [JsonProperty("minute", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Minute { get; set; }

        [JsonProperty("second", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Second { get; set; }
    }
}