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
    using DragDropPhoneApp.ApiConsumer;

    public partial class FirstPage : PhoneApplicationPage
    {
        public FirstPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApiService<string>.PingHost("http://localhost:61251/api/routesapi");
        }
    }
}