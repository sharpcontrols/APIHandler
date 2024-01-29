using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APIHandler
{
    public class RequestContent
    {
        private JObject _content { get; set; } = new();

        public RequestContent() { }
        public RequestContent(string serialized)
        {
            _content = JsonConvert.DeserializeObject<JObject>(serialized)!;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(_content);
        }

        public void AddNode<T>(string key, T value)
        {
            _content.Add(key, JToken.FromObject(value!));
        }

        public void RemoveNode(string key)
        {
            _content.Remove(key);
        }
    }
}
