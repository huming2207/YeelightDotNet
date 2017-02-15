using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YeelightHelper
{
    [JsonObject]
    public class YeelightResultJsonPayload
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "result", NullValueHandling = NullValueHandling.Include)]
        public List<object> PayloadParameters { get; set; }

        [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Include)]
        public YeelightResultJsonError Error { get; set; }
    }
}
