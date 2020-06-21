using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Calendarific.NetClient
{
    public class Holiday
    {
        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Country Country { get; set; }

        [JsonProperty("date", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Date Date { get; set; }

        [JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ObservableCollection<string> Type { get; set; }

        [JsonProperty("locations", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Locations { get; set; }

        [JsonProperty("states", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string States { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Holiday FromJson(string data)
        {
            return JsonConvert.DeserializeObject<Holiday>(data);
        }

    }
}