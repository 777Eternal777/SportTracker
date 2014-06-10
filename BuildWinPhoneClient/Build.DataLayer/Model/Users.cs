
using System;
using System.Collections.Generic;

namespace BuildSeller.Core.Model
{
    using Build.DataLayer.Model;

    using Newtonsoft.Json;

    [JsonObject]
    public class Users : Entity
    {

        [JsonProperty]
        public virtual string Login { get; set; }


        [JsonProperty]
        public virtual string Password { get; set; }


        [JsonProperty]
        public virtual DateTime RegisterDateTime { get; set; }


        [JsonProperty]
        public virtual List<Route> Routes { get; set; }
    }
}
