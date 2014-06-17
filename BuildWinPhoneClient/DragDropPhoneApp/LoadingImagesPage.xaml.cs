#region Using Directives

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Build.DataLayer.Enum;

using DragDropPhoneApp.ApiConsumer;
using DragDropPhoneApp.ApiModel;
using DragDropPhoneApp.Service;

using Microsoft.Phone.Controls;

using Newtonsoft.Json;

#endregion

namespace DragDropPhoneApp
{
    public partial class LoadingImagesPage : PhoneApplicationPage
    {
        
        #region Constructors and Destructors

        public LoadingImagesPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // new CustomProgressBar(this.ContentPanel,5);
            Task.Factory.StartNew(
                () =>
                    {
                        var das =
                            ApiService<ActivityType>.DownloadJsonWebClient(
                                ApiService<ActivityType>.uriRoutesApi.OriginalString);

                        var actList = JsonConvert.DeserializeObject<string[]>(das.Result);
                        if (actList == null)
                        {
                            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        }

                        var images = DataService.GetImagesNamesList(false);

                        var imagesToDownload = actList.Where(c => !images.Any(v => v == c.ToString()));
                        foreach (var value in imagesToDownload)
                        {
                            ApiService<DownloadedImag>.DownloadImage(value);
                        }
                    });

            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        #endregion
    }
}