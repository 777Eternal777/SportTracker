#region Using Directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

using Build.DataLayer.Enum;

using Newtonsoft.Json;

#endregion

namespace Build.DataLayer.Model
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Route
    {
        #region Constructors and Destructors

        public Route()
        {
            this.Points = new List<Points>();
        }

        #endregion

        #region Public Properties

        [JsonProperty]
        public ActivityType ActivityType { get; set; }

        [JsonProperty]
        public string ActivityTypeString { get; set; }

        [JsonProperty]
        public DateTime CreatedTime { get; set; }

        [JsonProperty]
        public TimeSpan Duration { get; set; }

        public BitmapImage Image
        {
            get
            {
                BitmapImage img = new BitmapImage();
                string filename = @"\Images\" + this.ActivityTypeString;
                Stream stream = null;
                using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isoStore.FileExists(filename))
                    {
                        stream = isoStore.OpenFile(filename, FileMode.Open, FileAccess.Read);
                    }
                }

                img.SetSource(stream);
                return img;
            }
        }

        [JsonProperty]
        public double Length { get; set; }

        [JsonProperty]
        public List<Points> Points { get; set; }

        [JsonProperty]
        public string UserName { get; set; }

        #endregion
    }

    [JsonObject]
    public class Points
    {
        #region Public Properties

        [JsonProperty]
        public double X { get; set; }

        [JsonProperty]
        public double Y { get; set; }

        #endregion
    }
}