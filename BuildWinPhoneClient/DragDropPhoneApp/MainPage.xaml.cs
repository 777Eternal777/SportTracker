namespace DragDropPhoneApp
{
    #region Using Directives

    using System;
    using System.Windows;

    using DragDropPhoneApp.Helpers;
    using DragDropPhoneApp.Service;
    using DragDropPhoneApp.ViewModel;

    using Microsoft.Phone.Controls;

    #endregion

    public partial class MainPage : PhoneApplicationPage
    {
      

        #region Constructors and Destructors
       
        public MainPage()
        {//var writeableBitmap = new WriteableBitmap((int)YourLongListSelector.RenderSize.Width, (int)LongListSelector.RenderSize.Height);

           // writeableBitmap.Render(YourLongListSelector, new ScaleTransform() { ScaleX = 1, ScaleY = 1 });
           // writeableBitmap.Invalidate();

        //    image.Source = writeableBitmap;
            this.InitializeComponent();
            this.DataContext = App.DataContext;
        }

        #endregion

        #region Methods

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Indicator.setLoadingIndicator(this, "Loading");

            App.DataContext.isInRealtyCreating = false;
          //  App.DataContext.photos = DataService.GetImages().Result;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.Relative));
        }

        #endregion
    }
}