#region Using Directives



#endregion
namespace Build.DataLayer.Model
{
    using System.Windows.Media.Imaging;

    using Build.DataLayer.Enum;

    public class Activity : ImageCard
    {
        #region Public Properties

        public BitmapImage Image { get; set; }
        public ActivityType ActivityType { get; set; }
        #endregion
    }
}