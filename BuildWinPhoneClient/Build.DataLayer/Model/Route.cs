using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.DataLayer.Model
{
    using System.Windows;

    using Build.DataLayer.Enum;

    using Newtonsoft.Json;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Route 
    {

        [JsonProperty]
        public string UserName { get; set; }
        [JsonProperty]
        public float Length { get; set; }
        [JsonProperty]
        public Points[] Points { get; set; }

        [JsonProperty]
        public DateTime CreatedTime { get; set; }

        [JsonProperty]
        public ActivityType ActivityType { get; set; }

    }
    [JsonObject]
    public class Points 
    {


        [JsonProperty]
        public Point Point { get; set; }
    }
}
