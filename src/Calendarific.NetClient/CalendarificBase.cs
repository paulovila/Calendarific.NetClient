using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Calendarific.NetClient
{
    public class CalendarificBase
    {
        private readonly ICalendarificConfig _calendarConfig;

        public CalendarificBase(ICalendarificConfig calendarConfig) => _calendarConfig = calendarConfig;
        public string BaseUrl => _calendarConfig.BaseUrl;

        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(15);

        internal Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken) => Task.FromResult(new HttpClient { Timeout = _timeout });
        internal void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
        {
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.All;
        }

        internal  Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken) => Task.FromResult(new HttpRequestMessage());

        public void PrepareRequest(HttpClient client, HttpRequestMessage request,
            System.Text.StringBuilder urlBuilder)
        {
            urlBuilder.Append("&api_key=");
            urlBuilder.Append(_calendarConfig.ApiKey);
        }
    }
}