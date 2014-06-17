#region Using Directives

using System;
using System.Windows;

using Windows.Foundation.Metadata;

using LinqToTwitter;

using Microsoft.Phone.Controls;

#endregion

namespace DragDropPhoneApp.ApiConsumer
{
    public static class TwitterApi
    {
        #region Static Fields

        private static string APIsecret = ResourceStrings.Strings.APIsecret;

        // Keep the "API secret" a secret. This key should never be human-readable in your application.
        private static string AccesToken = ResourceStrings.Strings.AccesToken;

        private static string AccesTokenSecret = ResourceStrings.Strings.AccesTokenSecret;

        private static string ApiKey = ResourceStrings.Strings.ApiKey;

        private static SingleUserAuthorizer auth = new SingleUserAuthorizer
        {
            CredentialStore =
                new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = ApiKey,
                    ConsumerSecret = APIsecret,
                    AccessToken = AccesToken,
                    AccessTokenSecret = AccesTokenSecret
                }
        };
        #endregion

        #region Public Methods and Operators

        public static void PostMessageWithImageToTwitter(string message, byte[] activity)
        {
            if (activity == null || activity.Length == 0)
            {
                return;
            }


            TwitterContext tweetContext = new TwitterContext(auth);

            var ds = tweetContext.TweetWithMediaAsync(message, false, activity).Result;
            if (ds != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    {
                        App.DataContext.IsLoading = false;
                        MessageBox.Show(ds.Text);
                        ((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(
                            new Uri("/RealtyList.xaml", UriKind.Relative));
                    });
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    {
                        App.DataContext.IsLoading = false;
                        ((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(
                            new Uri("/RealtyList.xaml", UriKind.Relative));
                    });
            }
        }

        #endregion
    }
}