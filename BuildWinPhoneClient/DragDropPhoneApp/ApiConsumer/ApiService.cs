namespace DragDropPhoneApp.ApiConsumer
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    using Build.DataLayer.Enum;
    using Build.DataLayer.Model;

    using DragDropPhoneApp.Service;
    using DragDropPhoneApp.ViewModel;

    using Microsoft.Phone.Controls;

    using Newtonsoft.Json;

    #endregion

    public class Imag
    {
        #region Fields

        [JsonProperty]
        public ActivityType ActivityType;

        [JsonProperty]
        public byte[] Content;

        #endregion
    }

    internal static class ApiService<T>
        where T : class
    {
        #region Static Fields

        public static Uri uriRoutesApi = new Uri("http://localhost:61251/api/routesapi");

        private static int imagesDownloaded;

        private static Uri uriUserApi = new Uri("http://localhost:61251/api/userapi");

        #endregion

        #region Public Methods and Operators

        public static void DownloadImage(string imgActivity)
        {
            WebClient client = new WebClient();

            client.Headers["Accept"] = "application/json";
            client.DownloadStringCompleted += (sender, args) =>
                {
                    Imag imag = null;
                    try
                    {
                        imag = JsonConvert.DeserializeObject<Imag>(args.Result);
                    }
                    catch (TargetInvocationException)
                    {
                        imagesDownloaded++;
                    }

                    if (imag != null && imag.ActivityType.Image != null && imag.ActivityType.Image.Length != 0)
                    {
                        DataService.SaveImage(imag.ActivityType.Type.ToString(), imag.ActivityType.Image);
                        if (App.DataContext.DownloadImageUnderNumberCompleted.ContainsKey(imagesDownloaded))
                        {
                            App.DataContext.DownloadImageUnderNumberCompleted[imagesDownloaded] = true;
                            imagesDownloaded++;
                        }
                    }
                };

            client.DownloadStringAsync(
                new Uri(uriRoutesApi.OriginalString + string.Format("?activity={0}", imgActivity)));
        }

        public static Task<string> DownloadJsonWebClient(string url)
        {
            var tcs = new TaskCompletionSource<string>();
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            WebClient client = new WebClient();

            client.DownloadStringCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        tcs.SetResult(e.Result);
                    }
                    else
                    {
                        tcs.SetException(e.Error);
                    }
                };

            client.Headers["Accept"] = "application/json";

            client.DownloadStringAsync(new Uri(url));

            return tcs.Task;

        }

        public static void GetRoutes()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => { App.DataContext.IsLoading = true; });

            WebClient client = new WebClient();

            client.Headers["Accept"] = "application/json";
            client.DownloadStringCompleted += RouteDownloadedCallback;

            client.DownloadStringAsync(
                new Uri(uriRoutesApi.OriginalString + string.Format("?userName={0}", App.DataContext.CurrentUser.Login)));
        }

        public static void Login(string login, string pass)
        {
            StartWebRequest(uriUserApi.OriginalString + string.Format("?login={0}&pass={1}", login, pass), null);
        }

        public static string PingHost(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            AsyncCallback callback = FinishPingRequest;
            request.BeginGetResponse(callback, request);
            return string.Empty;
        }

        public static void RouteDownloadedCallback(object s1, DownloadStringCompletedEventArgs e1)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        var realtys = JsonConvert.DeserializeObject<Route[]>(e1.Result);
                        if (realtys != null)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() => { App.DataContext.IsLoading = false; });

                            App.DataContext.Routes = realtys.ToList();
                        }
                    });
        }

        public static void SendPost(T gizmo, bool isRealtApi = true)
        {
            var serializedString = JsonConvert.SerializeObject(gizmo);

            DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(T));
            MemoryStream memStream = new MemoryStream();
            jsonData.WriteObject(memStream, serializedString);

            byte[] jsonDataToPost = memStream.ToArray();
            memStream.Close();

            var data1 = Encoding.UTF8.GetString(jsonDataToPost, 0, jsonDataToPost.Length);

            WebClient webClient = new WebClient();

            webClient.Headers["content-type"] = "application/json";
            if (isRealtApi)
            {
                webClient.UploadStringAsync(uriRoutesApi, "POST", data1);
            }
            else
            {
                webClient.UploadStringAsync(uriUserApi, "POST", data1);
            }
        }

        #endregion

        #region Methods

        private static void FinishPingRequest(IAsyncResult result)
        {
            try
            {
                HttpWebResponse response =
                    (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () =>
                            {
                                ((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(
                                    new Uri("/MainPage.xaml", UriKind.Relative));
                            });
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show("Server offline"); });
                }
            }
            catch (WebException e)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show("Server offline"); });
            }
        }

        private static void FinishWebRequest(IAsyncResult result)
        {
            try
            {
                HttpWebResponse response =
                    (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () =>
                            {
                                if (((PhoneApplicationFrame)Application.Current.RootVisual).DataContext is MainViewModel)
                                {
                                    (((PhoneApplicationFrame)Application.Current.RootVisual).DataContext as
                                     MainViewModel).IsLoading = false;
                                }

                                ((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(
                                    new Uri("/AllImagesPage.xaml", UriKind.Relative)); // AllImagesPage
                            });
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () => { MessageBox.Show("No user with such credentials"); });
                }
            }
            catch (WebException e)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show("No user with such credentials"); });
            }
        }

        private static void StartWebRequest(string url, AsyncCallback asyncCallback, string method = "GET")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            AsyncCallback callback;
            if (asyncCallback == null)
            {
                callback = FinishWebRequest;
            }
            else
            {
                callback = asyncCallback;
            }

            request.BeginGetResponse(callback, request);
        }

        #endregion
    }
}