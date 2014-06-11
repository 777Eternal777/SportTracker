using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.DataLayer.Model
{
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    using Build.DataLayer.Enum;

    using Newtonsoft.Json;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Route
    {
        public Route()
        {
            Points = new List<Points>();
        }

        public BitmapImage Image
        {
            get
            {
                BitmapImage img = new BitmapImage();
                string filename = @"\Images" + @"\" + this.ActivityType.ToString();
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
        public string UserName { get; set; }
        [JsonProperty]
        public double Length { get; set; }
        [JsonProperty]
        public List<Points> Points { get; set; }
        [JsonProperty]
        public TimeSpan Duration { get; set; }
       
        [JsonProperty]
        public DateTime CreatedTime { get; set; }

        [JsonProperty]
        public ActivityType ActivityType { get; set; }

    }
    [JsonObject]
    public class Points
    {


        [JsonProperty]
        public double X { get; set; }

        [JsonProperty]
        public double Y { get; set; }
    }
}
