using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.DataLayer.Enum
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ActivityType 
    {
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public byte[] Image { get; set; }

        [JsonProperty]
        public DateTime TimeCreated { get; set; }
    }
}
