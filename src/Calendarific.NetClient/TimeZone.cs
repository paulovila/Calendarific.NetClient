using Newtonsoft.Json;

namespace Calendarific.NetClient
{
    public class TimeZone
    {

        [JsonProperty("offset", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Offset { get; set; }

        [JsonProperty("zoneabb", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ZoneAbb { get; set; }

        [JsonProperty("zoneoffset", Required = Required.Always)]
        public int ZoneOffset { get; set; }

        [JsonProperty("zonedst", Required = Required.Always)]
        public int ZoneDST { get; set; }

        [JsonProperty("zonetotaloffset", Required = Required.Always)]
        public int ZoneTotalOffset { get; set; }
    }
}