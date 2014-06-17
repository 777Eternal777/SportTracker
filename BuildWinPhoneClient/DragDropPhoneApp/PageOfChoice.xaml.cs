#region Using Directives

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using DragDropPhoneApp.ApiConsumer;
using DragDropPhoneApp.Helpers;

using Microsoft.Phone.Controls;

using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion

namespace DragDropPhoneApp
{
    public partial class PageOfChoice : PhoneApplicationPage
    {
        #region Constructors and Destructors

        public PageOfChoice()
        {
            this.InitializeComponent();
            this.DataContext = App.DataContext;
        }

        #endregion

        #region Methods

        private void Not_to_share_Tap(object sender, GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RealtyList.xaml", UriKind.Relative));
        }

        private void Share_Tap(object sender, GestureEventArgs e)
        {
            Indicator.SetLoadingIndicator(this, "Sending tweet");
            var activite = App.DataContext.CurrentActivity;
            byte[] imageBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                WriteableBitmap btmMap = new WriteableBitmap(activite.Image);

                btmMap.SaveJpeg(ms, activite.Image.PixelWidth, activite.Image.PixelHeight, 0, 100);
                imageBytes = ms.ToArray();
            }

            App.DataContext.IsLoading = true;
            TaskFactory s = new TaskFactory();
            s.StartNew(
                () =>
                    {
                        TwitterApi.PostMessageWithImageToTwitter(
                            App.DataContext.CurrentActivity.ActivityType.Type + " "
                            + App.DataContext.CurrentActivity.TimeStamp, 
                            imageBytes);
                    });
        }

        #endregion
    }
}