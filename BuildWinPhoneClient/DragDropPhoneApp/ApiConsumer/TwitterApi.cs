using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropPhoneApp.ApiConsumer
{
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Windows;

    using Build.DataLayer.Model;

    using Microsoft.Phone.Net.NetworkInformation;

    using TweetSharp;

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
        public static void PostMessageWithImageToTwitter(string message, Activity activity)
        {


            if (NetworkInterface.GetIsNetworkAvailable())
            {
                //Obtain keys by registering your app on https://dev.twitter.com/apps
                var service = new TwitterService(ApiKey, APIsecret);
                service.AuthenticateWith(AccesToken, AccesTokenSecret);

             
                 service.SendTweetWithMedia(new SendTweetWithMediaOptions
                                                {
                                                    Status = message,

                                                },
                     (status, response) =>
                         {
                             
                         });
                //ScreenName is the profile name of the twitter user.

            }
            else
            {

                //    MessageBox.Show("Please check your internet connestion.");
            }
        }
    }
}
