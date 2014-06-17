#region Using Directives

using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Phone.Shell;

#endregion

namespace DragDropPhoneApp.Helpers
{
    public static class Indicator
    {
        #region Public Methods and Operators

        public static void SetLoadingIndicator(Page page, string text)
        {
            var progressIndicator = SystemTray.ProgressIndicator;
            if (progressIndicator != null)
            {
                return;
            }

            progressIndicator = new ProgressIndicator();

            SystemTray.SetProgressIndicator(page, progressIndicator);

            Binding binding = new Binding("IsLoading") { Source = page.DataContext };
            BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsVisibleProperty, binding);

            binding = new Binding("IsLoading") { Source = page.DataContext };
            BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsIndeterminateProperty, binding);

            progressIndicator.Text = text;
        }

        #endregion
    }
}