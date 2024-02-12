using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace SharpControls.APIHandler.REST
{
    public class RestConnection : IDisposable
    {
        private bool disposedValue;

        private HttpClient Client { get; set; } = new();
        public string Host { get { return _host; } set { _host = value.EndsWith("/") || value == "" ? value : value + "/";  } }
        private string _host;

        private string CombinedUrl(string url)
        {
            return Host + url;
        }

        public RestConnection(string host = "") {
            _host = host;
        }

        public RestConnection(HttpRequestHeaders headers, string host = "")
        {
            _host = host;
            Client.DefaultRequestHeaders.Clear();
            foreach(var header in headers)
            {
                Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        public RestConnection(Dictionary<string, string> headers, string host = "")
        {
            _host = host;
            Client.DefaultRequestHeaders.Clear();
            foreach(var header in headers)
            {
                Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        public RestConnection(ProductInfoHeaderValue userAgent, string host = "")
        {
            _host = host;
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.UserAgent.Add(userAgent);
        }

        public RestConnection(string userAgentName, string userAgentVersion, string host = "")
        {
            _host = host;
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.UserAgent.Add(new(userAgentName, userAgentVersion));
        }

        public RestConnection(HttpRequestHeaders headers, ProductInfoHeaderValue userAgent, string host = "")
        {
            _host = host;
            Client.DefaultRequestHeaders.Clear();
            foreach (var header in headers)
            {
                Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            Client.DefaultRequestHeaders.UserAgent.Add(userAgent);
        }

        public RestConnection(string userAgentName, string userAgentVersion, Dictionary<string, string> headers, string host = "")
        {
            _host = host;
            Client.DefaultRequestHeaders.Clear();
            foreach (var header in headers)
            {
                Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            Client.DefaultRequestHeaders.UserAgent.Add(new(userAgentName, userAgentVersion));
        }

        public RestConnection(string userAgentName, string userAgentVersion, string host = "", params KeyValuePair<string,string>[] headers)
        {
            _host = host;
            Client.DefaultRequestHeaders.Clear();
            foreach (var header in headers)
            {
                Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            Client.DefaultRequestHeaders.UserAgent.Add(new(userAgentName, userAgentVersion));
        }

        public void SetUserAgent(string name, string version = "1.0")
        {
            Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(name, version));
        }

        public void SetDefaultHeader(string key, string value)
        {
            Client.DefaultRequestHeaders.Add(key, value);
        }

        public void ClearDefaultHeaders()
        {
            Client.DefaultRequestHeaders.Clear();
        }

        public void ReloadClient()
        {
            Client = new();
        }

        public async Task<JObject> SendRequest(HttpMethod method, string uri, RequestContent content)
        {
            HttpRequestMessage msg = new(method, CombinedUrl(uri))
            {
                Content = new StringContent(content.ToString())
            };
            var response = await Client.SendAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("SendRequest failed => " + response.StatusCode);
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseBody)!;
        }

        public async Task<JObject> SendGet(string uri)
        {
            HttpRequestMessage msg = new(HttpMethod.Get, CombinedUrl(uri));
            var response = await Client.SendAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("SendGet failed => " + response.StatusCode);
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseBody)!;
        }

        public async Task<JObject> SendPost(string uri, RequestContent content)
        {
            HttpRequestMessage msg = new(HttpMethod.Post, CombinedUrl(uri));
            msg.Content = new StringContent(content.ToString());
            var response = await Client.SendAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("SendPost failed => " + response.StatusCode);
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseBody)!;
        }

        public async Task<JObject> SendPut(string uri, RequestContent content)
        {
            HttpRequestMessage msg = new(HttpMethod.Put, CombinedUrl(uri));
            msg.Content = new StringContent(content.ToString());
            var response = await Client.SendAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("SendPost failed => " + response.StatusCode);
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseBody)!;
        }

        public async Task<JObject> SendDelete(string uri, RequestContent content)
        {
            HttpRequestMessage msg = new(HttpMethod.Delete, CombinedUrl(uri));
            msg.Content = new StringContent(content.ToString());
            var response = await Client.SendAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("SendPost failed => " + response.StatusCode);
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseBody)!;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
