namespace DragDropPhoneApp.ApiConsumer
{
    #region Using Directives

    using System;
    using System.Windows;

    using LinqToTwitter;

    using Microsoft.Phone.Controls;

    #endregion

    public static class TwitterApi
    {
        #region Static Fields

        private static string APIsecret = "5eqgOqF1Ca2fUSL8MwZaL6F3GW5xlTKLEbHph46HUs0iLrPtL2";
                              // Keep the "API secret" a secret. This key should never be human-readable in your application.
        private static string AccesToken = "2558909828-qsFXEywYd2xjGo1b9pIANFvMjN2ANAOiSPsp2mp";

        private static string AccesTokenSecret = "29shsDxbFt0Koy17jygoxWeLN7jzapiNLOvki90lDtxIf";

        private static string ApiKey = "riMI9rvmpxdyQWFAJvxzUsusm";

        #endregion

        #region Public Methods and Operators

        public static void PostMessageWithImageToTwitter(string message, byte[] activity)
        {
            var auth = new SingleUserAuthorizer
                           {
                               CredentialStore =
                                   new SingleUserInMemoryCredentialStore
                                       {
                                           ConsumerKey = ApiKey, 
                                           ConsumerSecret =
                                               APIsecret, 
                                           AccessToken = AccesToken, 
                                           AccessTokenSecret =
                                               AccesTokenSecret
                                       }
                           };
            TwitterContext tweetContext = new TwitterContext(auth);

            byte[] imageBytes = activity;

            var ds = tweetContext.TweetWithMediaAsync(message, false, imageBytes).Result;
            if (ds != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                        {
                            MessageBox.Show(ds.Text);
                            ((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(
                                new Uri("/RealtyList.xaml", UriKind.Relative));
                        });
            }
        }

        #endregion
    }
}