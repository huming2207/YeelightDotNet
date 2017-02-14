using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YeelightHelper
{
    [JsonObject]
    public class CronJsonResult
    {
        [JsonProperty(PropertyName = "type")]
        public CronType CronType { get; set; }

        [JsonProperty(PropertyName = "delay")]
        public int DelayInMinutes { get; set; }

        [JsonProperty(PropertyName = "mix")]
        public int Mix { get; set; }
    }
}
