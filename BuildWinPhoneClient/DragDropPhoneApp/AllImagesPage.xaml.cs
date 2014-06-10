namespace DragDropPhoneApp
{
    #region Using Directives

    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Build.DataLayer.Model;

    using DragDropPhoneApp.Service;

    using Microsoft.Phone.Controls;

    #endregion

    public partial class AllImagesPage : PhoneApplicationPage
    {
        #region Constructors and Destructors

        public AllImagesPage()
        {
            this.InitializeComponent();
            this.DataContext = App.DataContext;
        }

        #endregion

        #region Methods

        private void ImagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sendr = sender as LongListSelector;
            if (sender != null)
            {
                if (sendr.SelectedItem == null)
                {
                    return;
                }

                var activity = sendr.SelectedItem as Activity;
                if (activity != null)
                {
                    App.DataContext.CurrentActivity = activity;

                    this.NavigationService.Navigate(new Uri("/Maps.xaml", UriKind.Relative));
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.DataContext.photos = DataService.GetImages().Result;
        }

        #endregion
    }
}