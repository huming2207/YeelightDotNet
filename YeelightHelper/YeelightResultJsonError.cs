using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YeelightHelper
{
    [JsonObject]
    public class YeelightResultJsonError
    {
        [JsonProperty(PropertyName = "code")]
        public int ErrorCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string ErrorMessage { get; set; }
    }
}
