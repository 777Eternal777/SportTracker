#region Using Directives

using System;

using Newtonsoft.Json;

#endregion

namespace Build.DataLayer.Enum
{
    [JsonObject]
    public class ActivityType
    {
        #region Public Properties

        [JsonProperty]
        public byte[] Image { get; set; }

        [JsonProperty]
        public DateTime TimeCreated { get; set; }

        [JsonProperty]
        public string Type { get; set; }

        #endregion
    }
}