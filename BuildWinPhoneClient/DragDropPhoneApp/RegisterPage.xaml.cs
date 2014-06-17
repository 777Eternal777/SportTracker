#region Using Directives

using System;
using System.Windows;
using System.Windows.Controls;

using BuildSeller.Core.Model;

using DragDropPhoneApp.ApiConsumer;

using Microsoft.Phone.Controls;

using Windows.Phone.System.Analytics;

using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion

namespace DragDropPhoneApp
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        #region Constructors and Destructors

        public RegisterPage()
        {
            this.InitializeComponent();
            this.DataContext = App.DataContext;
        }

        #endregion

        #region Methods

        private void Button_Tap(object sender, GestureEventArgs e)
        {
            if (string.IsNullOrEmpty(App.DataContext.CurrentUser.Login)
                || string.IsNullOrEmpty(App.DataContext.CurrentUser.Password))
            {
                MessageBox.Show("Logind and password cannot be empty");
                return;
            }

            App.DataContext.CurrentUser.RegisterDateTime = DateTime.Now;
            App.DataContext.CurrentUser.DeviceId = HostInformation.PublisherHostId;

            ApiService<Users>.SendPost(App.DataContext.CurrentUser, false);
            MessageBox.Show("Registration successfull");
            this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void Name_Tap(object sender, GestureEventArgs e)
        {
            if (sender is TextBox)
            {
                (sender as TextBox).SelectAll();
            }
        }

        #endregion
    }
}