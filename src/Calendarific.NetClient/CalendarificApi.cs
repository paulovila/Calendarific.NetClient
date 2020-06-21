//#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
//#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'

using System.Reflection;
using System.Runtime.Serialization;

#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."

namespace Calendarific.NetClient
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;
    using System = System;

    public interface ICalendarificApi
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">The year you want to return the holidays. We currently support both historical and future years until 2049. The year must be specified as a number eg, 2019</param>
        /// <param name="country">The country parameter must be in the iso-3166 format as specified in the document here. To view a list of countries and regions we support, visit our list of supported countries.</param>
        /// <param name="day">Optional: Limits the number of holidays to a particular day. Must be passed as the numeric value of the day [1..31].</param>
        /// <param name="month">Optional: Limits the number of holidays to a particular month. Must be passed as the numeric value of the month [1..12].</param>
        /// <param name="location">Optional: We support multiple counties, states and regions for all the countries we support. This optional parameter allows you to limit the holidays to a particular state or region. The value of field is iso-3166 format of the state. View a list of supported countries and states. An example is, for New York state in the United States, it would be us-ny</param>
        /// <param name="holidayType">Optional:  We support multiple types of holidays and observances. This parameter allows users to return only a particular type of holiday or event. By default, the API returns all holidays. Below is the list of holiday types supported by the API and this is how to reference them.
        /// *national: Returns public, federal and bank holidays
        /// *local: Returns local, regional and state holidays
        /// *religious: Return religious holidays: buddhism, christian, hinduism, muslim, etc
        /// *observance: Observance, Seasons, Times
        /// </param>
        /// <param name="language">Premium Optional: Returns the name of the holiday in the official language of the country if available. This defaults to english. This must be passed as the 2-letter ISO639 Language Code. An example is to return all the names of france holidays in french you can just add the parameter like this: language=fr</param>
        /// <param name="uuid">Premium Optional: Returns a UUID for every holiday returned in the response [true or false].</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="CalendarificException">A server side error occurred.</exception>
        /// <returns></returns>
        System.Threading.Tasks.Task<Collection<Holiday>> HolidaysAsync(int year, string country,
            int? day = null,
            int? month = null,
            string location = null,
            HolidayType? holidayType = null,
            string language = null,
            bool? uuid = null,
            System.Threading.CancellationToken cancellationToken = default);
    }

    public class CalendarificApi : CalendarificBase, ICalendarificApi
    {
        private readonly System.Lazy<JsonSerializerSettings> _settings;
        public CalendarificApi(ICalendarificConfig configuration) : base(configuration) => _settings = new System.Lazy<JsonSerializerSettings>(CreateSerializerSettings);
        private JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }

        protected JsonSerializerSettings JsonSerializerSettings => _settings.Value;
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="CalendarificException">A server side error occurred.</exception>
        public async System.Threading.Tasks.Task<Collection<Holiday>> HolidaysAsync(int year, string country,
            int? day = null,
            int? month = null,
            string location = null,
            HolidayType? holidayType = null,
            string language = null,
            bool? uuid = null,
            System.Threading.CancellationToken cancellationToken = default)
        {
            if (country == null)
                throw new System.ArgumentNullException(nameof(country));

            var urlBuilder = new System.Text.StringBuilder();

            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/holidays?");
            urlBuilder.Append(System.Uri.EscapeDataString("year") + "=").Append(System.Uri.EscapeDataString(ConvertToString(year, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Append(System.Uri.EscapeDataString("country") + "=").Append(System.Uri.EscapeDataString(ConvertToString(country, System.Globalization.CultureInfo.InvariantCulture))).Append("&");

            void OptionalAppend(string parameter, string s)
            {
                if (!string.IsNullOrEmpty(s))
                    urlBuilder.Append(System.Uri.EscapeDataString(parameter) + "=")
                        .Append(System.Uri.EscapeDataString(ConvertToString(s, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            OptionalAppend("day", day?.ToString());
            OptionalAppend("month", month?.ToString());
            OptionalAppend("location", location);
            OptionalAppend("type", holidayType?.ToString());
            OptionalAppend("language", language);
            OptionalAppend("uuid", uuid?.ToString());

            urlBuilder.Length--;

            var client = await CreateHttpClientAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                using (var request = await CreateHttpRequestMessageAsync(cancellationToken).ConfigureAwait(false))
                {
                    request.Method = new System.Net.Http.HttpMethod("GET");
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client, request, urlBuilder);
                    var url = urlBuilder.ToString();
                    request.RequestUri = new System.Uri(url, System.UriKind.RelativeOrAbsolute);

                    var response = await client.GetAsync(request.RequestUri, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content?.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }

                        var status = ((int)response.StatusCode).ToString();
                        if (status == "200")
                        {
                            var objectResponse = await ReadObjectResponseAsync<m>(response, headers).ConfigureAwait(false);
                            return objectResponse.Object.response.Holidays;
                        }
                        else
                        if (status != "200" && status != "204")
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new CalendarificException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", (int)response.StatusCode, responseData, headers, null);
                        }

                        return default;
                    }
                    finally
                    {
                        response.Dispose();
                    }
                }
            }
            finally
            {
                client.Dispose();
            }
        }

        protected readonly struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                Object = responseObject;
                Text = responseText;
            }

            public T Object { get; }

            public string Text { get; }
        }

        public bool ReadResponseAsString { get; set; }

        protected virtual async System.Threading.Tasks.Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(System.Net.Http.HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers)
        {
            if (response?.Content == null)
            {
                return new ObjectResponseResult<T>(default, string.Empty);
            }

            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var typedBody = JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, responseText);
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new CalendarificException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var streamReader = new System.IO.StreamReader(responseStream))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var serializer = JsonSerializer.Create(JsonSerializerSettings);
                        var typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new CalendarificException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }

        private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is System.Enum)
            {
                string name = System.Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = value.GetType().GetTypeInfo().GetDeclaredField(name);
                    if (field != null)
                    {
                        if (field.GetCustomAttribute(typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute)
                        {
                            return attribute.Value ?? name;
                        }
                    }

                    return System.Convert.ToString(System.Convert.ChangeType(value, System.Enum.GetUnderlyingType(value.GetType()), cultureInfo));
                }
            }
            else if (value is bool)
            {
                return System.Convert.ToString(value, cultureInfo)?.ToLowerInvariant();
            }
            else if (value is byte[] bytes)
            {
                return System.Convert.ToBase64String(bytes);
            }
            else if (value != null && value.GetType().IsArray)
            {
                var array = System.Linq.Enumerable.OfType<object>((System.Array)value);
                return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            return System.Convert.ToString(value, cultureInfo);
        }
    }
}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore 472
#pragma warning restore 114
#pragma warning restore 108