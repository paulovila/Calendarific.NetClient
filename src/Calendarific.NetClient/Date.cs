using System;
using Newtonsoft.Json;

namespace Calendarific.NetClient
{
    public class Date
    {
        [JsonProperty("iso", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public object Iso { get; set; }

        [JsonProperty("datetime", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Datetime Datetime { get; set; }

        [JsonProperty("timezone", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public TimeZone Timezone { get; set; }
    }
}