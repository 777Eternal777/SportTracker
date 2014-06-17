#region Using Directives

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Build.DataLayer.Model;

using DragDropPhoneApp.ApiConsumer;
using DragDropPhoneApp.Helpers;

using Microsoft.Phone.Controls;

using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion

namespace DragDropPhoneApp
{
    public partial class RealtyList : PhoneApplicationPage
    {
        #region Constructors and Destructors

        public RealtyList()
        {
            this.InitializeComponent();
            this.DataContext = App.DataContext;
        }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApiService<Route>.GetRoutes();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Indicator.SetLoadingIndicator(this, "Loading routes");
        }
        #endregion
    }
}