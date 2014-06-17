#region Using Directives

using Build.DataLayer.Enum;

using Newtonsoft.Json;

#endregion

namespace DragDropPhoneApp.ApiModel
{
    public class DownloadedImag
    {
        #region Fields

        [JsonProperty]
        public ActivityType ActivityType;

        [JsonProperty]
        public byte[] Content;

        #endregion
    }
}