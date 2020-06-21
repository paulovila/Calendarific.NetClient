using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Calendarific.NetClient
{
    internal class ResponseHolidays
    {
        [JsonProperty("holidays", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ObservableCollection<Holiday> Holidays { get; set; }
    }
}