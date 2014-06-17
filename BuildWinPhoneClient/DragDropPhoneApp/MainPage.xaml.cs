#region Using Directives

using System;
using System.Windows;

using DragDropPhoneApp.Helpers;

using Microsoft.Phone.Controls;

#endregion

namespace DragDropPhoneApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Constructors and Destructors

        public MainPage()
        {
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
            Indicator.SetLoadingIndicator(this, "Loading");
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.Relative));
        }

        #endregion
    }
}