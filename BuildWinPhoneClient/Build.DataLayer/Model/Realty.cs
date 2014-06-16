namespace Build.DataLayer.Model
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Windows.Media.Imaging;

    using BuildSeller.Core.Model;

    using Newtonsoft.Json;

    #endregion

    [JsonObject]
    public class Realty : Entity
    {
        #region Public Properties

        [JsonProperty]
        public string Address { get; set; }

        [JsonProperty]
        public BuildCategories BuildCategory { get; set; }

        [JsonProperty]
        public DateTime Created { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public bool IsForRent { get; set; }

        [JsonProperty]
        public bool IsSold { get; set; }

        [JsonProperty]
        public double MapPosX { get; set; }

        [JsonProperty]
        public double MapPosY { get; set; }

        [JsonProperty]
        public string Named { get; set; }

        [JsonProperty]
        public Users Owner { get; set; }

        [JsonProperty]
        public byte[] Picture { get; set; }

        public BitmapImage PictureSource
        {
            get
            {
                BitmapImage biImg = new BitmapImage();
                if (this.Picture == null)
                {
                    return null;
                }

                MemoryStream ms = new MemoryStream(this.Picture);

                biImg.SetSource(ms);
                return biImg;
            }

            set
            {
                if (value != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        WriteableBitmap btmMap = new WriteableBitmap(value);

                        btmMap.SaveJpeg(ms, value.PixelWidth, value.PixelHeight, 0, 100);

                        this.Picture = ms.ToArray();
                    }
                }
            }
        }

        [JsonProperty]
        public decimal Price { get; set; }

        [JsonProperty]
        public float Square { get; set; }

        #endregion
    }
}