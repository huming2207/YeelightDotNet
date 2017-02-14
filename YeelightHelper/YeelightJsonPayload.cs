using Newtonsoft.Json;
using System.Collections.Generic;

namespace YeelightHelper
{
    [JsonObject]
    public class YeelightJsonPayload
    {
        [JsonProperty(PropertyName = "id")]
        public int PayloadId { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string PayloadMethod { get; set; }

        [JsonProperty(PropertyName = "params")]
        public List<object> PayloadParameters { get; set; }
    }
}
