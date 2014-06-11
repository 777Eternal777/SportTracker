using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropPhoneApp.ApiConsumer
{
    using System.Globalization;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Net;
    using System.Security.Cryptography;
    using System.Windows;
    using System.Windows.Media.Imaging;

    using Build.DataLayer.Model;

    using Hammock;
    using Hammock.Authentication.OAuth;
    using Hammock.Web;

    using LinqToTwitter;

    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Net.NetworkInformation;
    using Microsoft.Xna.Framework.Media;

    using TweetSharp;

    /*   public partial class TweetPhoto 
     {
         private readonly RestClient _client;
         private readonly OAuthCredentials _credentials;
         private readonly Picture _latestPicture;
         private static string AccesToken = "2558909828-qsFXEywYd2xjGo1b9pIANFvMjN2ANAOiSPsp2mp";

         private static string AccesTokenSecret = "29shsDxbFt0Koy17jygoxWeLN7jzapiNLOvki90lDtxIf";
         private static string ApiKey = "riMI9rvmpxdyQWFAJvxzUsusm";

         private static string APIsecret = "5eqgOqF1Ca2fUSL8MwZaL6F3GW5xlTKLEbHph46HUs0iLrPtL2"; //Keep the "API secret" a secret. This key should never be human-readable in your application.
      
    
       public TweetPhoto()
         {
           

             _credentials = new OAuthCredentials
             {
                 Type = OAuthType.ProtectedResource,
                 SignatureMethod = OAuthSignatureMethod.HmacSha1,
                 ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                 ConsumerKey = ApiKey,
                 ConsumerSecret = APIsecret,
                 Token = AccesToken,
                 TokenSecret = AccesTokenSecret,
                
             };
                   _client = new RestClient
             {
                 Authority = "http://api.twitter.com",
                 HasElevatedPermissions = true
             };

          
          //   _latestPicture = mediaLibrary.Pictures.Where(x => x.Name.Contains("PixImg_")).OrderByDescending(x => x.Date).FirstOrDefault();
             if (_latestPicture == null)
             {
                 // TODO handle case where no pictures have been taken yet
             }

             var bitmapImage = new BitmapImage();
         
             //Image.Source = bitmapImage;
         }

         public void OnTweetButtonClicked(Stream picture)
         {
             picture.Seek(0, SeekOrigin.Begin);
             var twitterRequest = new RestRequest
             {
                 Credentials = _credentials,
                 Path = "/1.1/statuses/update_with_media.json",
                 Method = WebMethod.Post
             };

             twitterRequest.AddParameter("status", "Asdasd");
             twitterRequest.AddFile("media[]", Guid.NewGuid().ToString(), picture, "image/jpeg");

             _client.BeginRequest(twitterRequest, NewTweetCompleted);
         }

         private void NewTweetCompleted(RestRequest request, RestResponse response, object userstate)
         {
             // We want to ensure we are running on our thread UI
             Deployment.Current.Dispatcher.BeginInvoke(() =>
             {
                 if (response.StatusCode == HttpStatusCode.OK)
                 {
                     MessageBox.Show("Tto Twitter");
                 }
                 else
                 {
                     MessageBox.Show(response.StatusCode + " " + response.StatusDescription );
                 }
             });
         }

      
     }*/
    public static class TwitterApi
    {
        

        private static string AccesToken = "2558909828-qsFXEywYd2xjGo1b9pIANFvMjN2ANAOiSPsp2mp";

        private static string AccesTokenSecret = "29shsDxbFt0Koy17jygoxWeLN7jzapiNLOvki90lDtxIf";
        private static string ApiKey = "riMI9rvmpxdyQWFAJvxzUsusm";

        private static string APIsecret = "5eqgOqF1Ca2fUSL8MwZaL6F3GW5xlTKLEbHph46HUs0iLrPtL2"; //Keep the "API secret" a secret. This key should never be human-readable in your application.
        public static void PostMessageToTwitter(string message)
        {
            
            
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //Obtain keys by registering your app on https://dev.twitter.com/apps
                var service = new TwitterService(ApiKey, APIsecret);
                service.AuthenticateWith(AccesToken, AccesTokenSecret);
              
                service.SendTweet(new SendTweetOptions
                                      {
                                          Status = message,
                                          
                                      },
                    (status, response) =>
                        {
                           // Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show(response.Response + response.StatusCode); });
                           
                        } );
       //   service.SendTweetWithMedia(new SendTweetWithMediaOptions
                                       //       {
                                              //    Images = 
                                      //        } );
                //ScreenName is the profile name of the twitter user.
              
            }
            else
            {

            //    MessageBox.Show("Please check your internet connestion.");
            }
        }
        public static void uploadPhoto(Stream photoStream, string photoName)
{
var credentials = new OAuthCredentials
        {
            Type = OAuthType.ProtectedResource,
            SignatureMethod = OAuthSignatureMethod.HmacSha1,
            ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
            ConsumerKey = ApiKey,
            ConsumerSecret = APIsecret,
            Token = AccesToken,
            TokenSecret = AccesTokenSecret,
            Version = "1.0a"
        };


        RestClient restClient = new RestClient
        {
            Authority = "https://upload.twitter.com",
            HasElevatedPermissions = true,
            Credentials = credentials,
            Method = WebMethod.Post
         };
         RestRequest restRequest = new RestRequest
         {
            Path = "1/statuses/update_with_media.json"
         };

         restRequest.AddParameter("status", "test");
         restRequest.AddFile("media[]", photoName, photoStream, "image/jpg");
         restClient.BeginRequest(restRequest, new RestCallback(PostTweetRequestCallback));

}

    



private static void PostTweetRequestCallback(RestRequest request, Hammock.RestResponse response, object obj)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show(response.Content + response.StatusCode); });
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
        //Success code
        }
}

        public static void PostMessageWithImageToTwitter(string message, byte[] activity)
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = ApiKey,
                    ConsumerSecret = APIsecret,
                    AccessToken = AccesToken,
                    AccessTokenSecret =AccesTokenSecret
                }
            };
            LinqToTwitter.TwitterContext adsa = new TwitterContext(auth);
            string status =
              message +
               DateTime.Now.ToString(CultureInfo.InvariantCulture);



        //    var tweet1 =  adsa.TweetAsync("asdsdasdsadsd").Result;
          //  if (tweet1 != null)
            //    Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show(tweet1.Text); });

        //    return;
            const bool PossiblySensitive = false;
            const decimal Latitude = 37.78215m; //37.78215m;
            const decimal Longitude = -122.40060m;// -122.40060m;
            const bool DisplayCoordinates = false;
            const string PlaceID = null;
            const string ReplaceThisWithYourImageLocation =
                @"..\..\images\200xColor_2.png";
            byte[] imageBytes = activity;
           

       var ds=     adsa.TweetWithMediaAsync(message, false, imageBytes).Result;
       if (ds != null)
           Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show(ds.Text); });
            return;
            Status tweet =  adsa.TweetWithMediaAsync(
                status, PossiblySensitive, Latitude, Longitude,
                PlaceID, DisplayCoordinates, imageBytes).Result;
              if (tweet != null)
                  Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show(tweet.Text); });
       /*     if (NetworkInterface.GetIsNetworkAvailable())
            {
                //Obtain keys by registering your app on https://dev.twitter.com/apps
                var service = new TwitterService(ApiKey, APIsecret);
                service.AuthenticateWith(AccesToken, AccesTokenSecret);
                var images = new Dictionary<string, Stream>();
                MemoryStream ms = new MemoryStream();
                 
                        WriteableBitmap btmMap = new WriteableBitmap
                            (activity.Image);
                       
                        Extensions.SaveJpeg(btmMap, ms,
                            activity.Image.PixelWidth, activity.Image.PixelHeight, 0, 100);


                        images.Add(activity.ActivityType.ToString(), ms);
            
                 service.SendTweetWithMedia(new SendTweetWithMediaOptions
                                                {
                                                    Status = message,
                                                    Images = images,
                                                    
                                                },
                     (status, response) =>
                         {
                             Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show(response.Response + response.StatusCode); });
                           
                         });
                //ScreenName is the profile name of the twitter user.

            }
            else
            {

                //    MessageBox.Show("Please check your internet connestion.");
            }*/
        }
    }
}
