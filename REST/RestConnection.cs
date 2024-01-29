using APIHandler.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace APIHandler.REST
{
    public static class RestConnection
    {

        private static HttpClient _client { get; set; }


        public static void SetUserAgent(string name, string version = "1.0")
        {
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(name, version));
        }

        public static void SetDefaultHeader(string key, string value)
        {
            _client.DefaultRequestHeaders.Add(key, value);
        }

        public static void Init()
        {
            _client = new HttpClient();
        }

        public static async Task<JObject> SendRequest(HttpMethod method, string uri, RequestContent content)
        {
            HttpRequestMessage msg = new(method, uri);
            msg.Content = new StringContent(content.ToString());
            var response = await _client.SendAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("SendRequest failed => " + response.StatusCode);
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseBody)!;
        }
    }
}
