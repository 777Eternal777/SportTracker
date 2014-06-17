#region Using Directives

using System.Windows.Media.Imaging;

using Build.DataLayer.Enum;

#endregion

namespace Build.DataLayer.Model
{
    public class Activity : ImageCard
    {
        #region Public Properties

        public ActivityType ActivityType { get; set; }

        public BitmapImage Image { get; set; }

        #endregion
    }
}