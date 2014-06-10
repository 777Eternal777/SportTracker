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
    using Build.DataLayer.Model;

    using DragDropPhoneApp.Service;
    using DragDropPhoneApp.ViewModel;

    public partial class AllImagesPage : PhoneApplicationPage
    {
        #region Fields


        #endregion

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
           /// return;
            if (sender !=null)
            {
               
                if (sendr.SelectedItem == null)
                {
                    return;
                }
                var activity = sendr.SelectedItem as Activity;
                if (activity != null)
                {
                    App.DataContext.CurrentActivity = activity;

                    //   App.DataContext.CurrentRealty.PictureSource = (sendr.SelectedItem as Photo).Image;

                    // sendr.SelectedItem = null;
                        this.NavigationService.Navigate(new Uri("/Maps.xaml", UriKind.Relative));
                }
            }
        }

        #endregion

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.DataContext.photos = DataService.GetImages().Result;
        }
    }
}