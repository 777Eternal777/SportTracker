#region Using Directives

using System;
using System.Collections.Generic;

using Build.DataLayer.Model;

using Newtonsoft.Json;

#endregion

namespace BuildSeller.Core.Model
{
    [JsonObject]
    public class Users : Entity
    {
        #region Public Properties

        [JsonProperty]
        public virtual string DeviceId { get; set; }

        [JsonProperty]
        public virtual string Login { get; set; }

        [JsonProperty]
        public virtual string Password { get; set; }

        [JsonProperty]
        public virtual DateTime RegisterDateTime { get; set; }

        [JsonProperty]
        public virtual List<Route> Routes { get; set; }

        #endregion
    }
}