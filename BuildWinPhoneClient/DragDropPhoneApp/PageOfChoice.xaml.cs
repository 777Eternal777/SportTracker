using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace DragDropPhoneApp
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.Helpers;
    using DragDropPhoneApp.ViewModel;

    public partial class PageOfChoice : PhoneApplicationPage
    {
        public PageOfChoice()
        {
            InitializeComponent();
            this.DataContext = App.DataContext;
        }

        private void Share_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Indicator.setLoadingIndicator(this, "Loggin in");
            var activite = App.DataContext.CurrentActivity;
            byte[] imageBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                WriteableBitmap btmMap = new WriteableBitmap
                    (activite.Image);

                Extensions.SaveJpeg(btmMap, ms,
                   activite.Image.PixelWidth, activite.Image.PixelHeight, 0, 100);
                imageBytes = ms.ToArray();

            }
            App.DataContext.IsLoading = true;
            TaskFactory s =new TaskFactory();
            s.StartNew(
                () =>
                    {
                        TwitterApi.PostMessageWithImageToTwitter(
                            App.DataContext.CurrentActivity.ActivityType.Type.ToString() + " "
                            + App.DataContext.CurrentActivity.TimeStamp,
                          imageBytes);
                    });

        }

        private void Not_to_share_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            this.NavigationService.Navigate(new Uri("/RealtyList.xaml", UriKind.Relative));
        }
    }
}