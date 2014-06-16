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
    using System.Device.Location;
    using System.Diagnostics;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    using Windows.Devices.Geolocation;

    using Build.DataLayer.Model;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.Service;
    using DragDropPhoneApp.ViewModel;

    using Microsoft.Phone.Maps.Controls;
    using Microsoft.Phone.Maps.Services;
    using Microsoft.Xna.Framework.Media;

    using Route = Microsoft.Phone.Maps.Services.Route;

    public partial class MapPage : PhoneApplicationPage
    {
        #region Fields

        private MapOverlay DestinationMarker;

        private bool DestinationRevGeoNow;

        private MapOverlay OriginMarker;

        private List<MapOverlay> MapMarkersList = new List<MapOverlay>();

        private RouteOptimization optimization = RouteOptimization.MinimizeTime;

        private TravelMode travelMode = TravelMode.Driving;

        private bool draggingNow;

        private RouteQuery geoQ;

        private ReverseGeocodeQuery geoRev;

        private MapRoute lastRoute;

        private MapLayer markerLayer;

        private MapOverlay selectedMarker;
        #endregion


        #region Constructors and Destructors

        public MapPage()
        {
            this.InitializeComponent();
            DataContext = App.DataContext;
            Touch.FrameReported += this.Touch_FrameReported;

            this.map1.ZoomLevelChanged += this.map1_ZoomLevelChanged;

            this.geoRev = new ReverseGeocodeQuery();
            this.geoRev.QueryCompleted += this.geoRev_QueryCompleted;

            this.geoQ = new RouteQuery();
            this.geoQ.QueryCompleted += this.geoQ_QueryCompleted;
            this.markerLayer = new MapLayer();

            this.map1.Layers.Add(this.markerLayer);
           AddPlusMinusButtons();
        }

        #endregion

        #region Methods
        private async void StartGeoLoc()
        {


            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.MovementThreshold = 20;
            watcher.Start();
            watcher.PositionChanged += (o, args) =>
                {
                    this.DestinationMarker.GeoCoordinate = watcher.Position.Location;
                    Start_ReverceGeoCoding(this.DestinationMarker);
                    Start_ReverceGeoCoding(this.OriginMarker);
                    StartGeoQ();
                };

        }
        private void AddResultToMap(GeoCoordinate origin, GeoCoordinate destination)
        {
            if (this.markerLayer != null)
            {
                this.map1.Layers.Remove(this.markerLayer);
                this.markerLayer = null;
            }

            this.OriginMarker = this.MakeDotMarker(origin, false);
            this.DestinationMarker = this.MakeDotMarker(destination, true);
            this.map1.SetView(origin, this.map1.ZoomLevel);
            this.markerLayer = new MapLayer();
            this.map1.Layers.Add(this.markerLayer);
            this.markerLayer.Add(this.OriginMarker);
            this.markerLayer.Add(this.DestinationMarker);

        }

        private void StartGeoQ()
        {
            if (MapMarkersList.Count < 2)
            {
                return;
            }
            if (this.geoQ.IsBusy)
            {
                this.geoQ.CancelAsync();
            }

            this.geoQ.InitialHeadingInDegrees = this.map1.Heading;

            this.geoQ.RouteOptimization = this.optimization;
            this.geoQ.TravelMode = this.travelMode;

            List<GeoCoordinate> MyWayPoints = new List<GeoCoordinate>();
            foreach (var marker in MapMarkersList)
            {
                MyWayPoints.Add(marker.GeoCoordinate);
            }


            this.geoQ.Waypoints = MyWayPoints;
            if (!geoQ.IsBusy)
                this.geoQ.QueryAsync();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (sender == this.GetRouteBtn)
            {
                StartGeoQ();
            }
        }

        private void AddPlusMinusButtons()
        {
            Ellipse circle = new Ellipse();

            circle.Stroke = new SolidColorBrush(Colors.Transparent);
            Grid grid = new Grid
            {
                Width = 60,
                Height = 60
            };
            TextBlock plus = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "+"
            };
            TextBlock minus = new TextBlock
          {
              VerticalAlignment = VerticalAlignment.Center,
              HorizontalAlignment = HorizontalAlignment.Center,
              Text = "-"
          };
            circle.StrokeThickness = 25;
            circle.Opacity = 0.8;
            circle.Height = 50;
            circle.Width = 50;
            grid.Children.Add(circle);
            grid.Children.Add(plus);
            grid.Tap += (sender, args) =>
            {
                if (this.map1.ZoomLevel < 20)
                {
                    this.map1.ZoomLevel++;
                }
            };
            Canvas.SetZIndex(grid, 10);
            grid.Margin = new Thickness(10, 163, 0, 0);
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.HorizontalAlignment = HorizontalAlignment.Left;

            //    this.ContentPanel.Children.Add(grid);

        }
        private MapOverlay MakeDotMarker(GeoCoordinate location, bool isDestination)
        {
            MapOverlay marker = new MapOverlay();

            marker.GeoCoordinate = location;

            Ellipse circle = new Ellipse();

            circle.Fill = new SolidColorBrush(Colors.Yellow);
            circle.Stroke = new SolidColorBrush(Colors.Red);
            Grid grid = new Grid
                            {
                                Width = 60,
                                Height = 60
                            };
            TextBlock num = new TextBlock
                                {
                                    VerticalAlignment = VerticalAlignment.Center,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    Text = (1 + this.MapMarkersList.Count).ToString()
                                };
            circle.StrokeThickness = 25;
            circle.Opacity = 0.8;
            circle.Height = 50;
            circle.Width = 50;
            grid.Children.Add(circle);
            grid.Children.Add(num);
            marker.Content = grid;
            marker.PositionOrigin = new Point(0.5, 0.5);
            circle.MouseLeftButtonDown += this.textt_MouseLeftButtonDown;
            this.markerLayer.Add(marker);
            this.MapMarkersList.Add(marker);
            return marker;
        }

        private void Start_ReverceGeoCoding(MapOverlay Marker)
        {
            if (this.geoRev.IsBusy != true && (Marker != null))
            {
                if (Marker == this.DestinationMarker)
                {
                    this.DestinationRevGeoNow = true;
                    this.DestinationTitle.Text = string.Empty;
                }
                else
                {
                    this.DestinationRevGeoNow = false;
                    this.OriginTitle.Text = string.Empty;
                }

                this.geoRev.GeoCoordinate = Marker.GeoCoordinate;
                this.geoRev.QueryAsync();
            }
        }

        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            try
            {
                TouchPoint tPoint = e.GetPrimaryTouchPoint(this.map1);
                if (tPoint.Action == TouchAction.Down)
                {
                    var marker = MakeDotMarker(this.map1.ConvertViewportPointToGeoCoordinate(tPoint.Position), false);

                    //this.selectedMarker.GeoCoordinate = this.map1.ConvertViewportPointToGeoCoordinate(tPoint.Position);
                    this.Start_ReverceGeoCoding(marker);
                    StartGeoQ();
                }
                
             
            }
            catch (ArgumentException)
            {


            }
        }

        private int failedQueriesCount = 0;

        private bool allfailed;

        private double routeLength;

        private TimeSpan routeDuration;
        private void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (allfailed)
            {
                this.MapMarkersList.RemoveAt(MapMarkersList.Count - 1);
                allfailed = false;
            }
            if (this.lastRoute != null)
            {
                this.map1.RemoveRoute(this.lastRoute);
                this.lastRoute = null;
            }

            try
            {
                Route myRoute = e.Result;

                this.lastRoute = new MapRoute(myRoute);

                this.map1.AddRoute(this.lastRoute);
                this.map1.SetView(e.Result.BoundingBox);
                routeLength = myRoute.LengthInMeters;
                MessageBox.Show(
                    "Distance: " + (routeLength / 1000) + " km, Estimated traveltime: "
                    + myRoute.EstimatedDuration);
                routeDuration = myRoute.EstimatedDuration;
                failedQueriesCount = 0;
            }
            catch (TargetInvocationException)
            {
                Thread.Sleep(1000);
                //  Debug.WriteLine("wrong data to query");
                failedQueriesCount++;
                if (failedQueriesCount < 5)
                {
                    geoQ_QueryCompleted(sender, e);
                }
                else
                {
                    allfailed = true;
                }
            }
            catch (InvalidOperationException)
            {

            }

        }

        private void geoRev_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {


            string GeoStuff = string.Empty;

            if (e.Result.Count() > 0)
            {
                if (e.Result[0].Information.Address.Street.Length > 0)
                {
                    GeoStuff = e.Result[0].Information.Address.Street;//GeoStuff +

                    if (e.Result[0].Information.Address.HouseNumber.Length > 0)
                    {
                        GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.HouseNumber;
                    }
                }

                if (e.Result[0].Information.Address.City.Length > 0)
                {
                    if (GeoStuff.Length > 0)
                    {
                        GeoStuff = GeoStuff + ",";
                    }

                    GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.City;

                    if (e.Result[0].Information.Address.Country.Length > 0)
                    {
                        GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.Country;
                    }
                }
                else if (e.Result[0].Information.Address.Country.Length > 0)
                {
                    if (GeoStuff.Length > 0)
                    {
                        GeoStuff = GeoStuff + ",";
                    }
                    GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.Country;
                }
            }
            if (this.DestinationRevGeoNow)
            {
                this.DestinationTitle.Text = GeoStuff;
            }
            else
            {
                this.OriginTitle.Text = GeoStuff;
            }
        }

        private void map1_ZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {


        }

        private void textt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse clickedOne = sender as Ellipse;
            if (clickedOne != null && this.OriginMarker != null && this.DestinationMarker != null)
            {
                if (this.DestinationMarker.Content == clickedOne)
                {
                    this.selectedMarker = this.DestinationMarker;
                    this.draggingNow = true;
                    this.map1.IsEnabled = false;
                }
                else if (this.OriginMarker.Content == clickedOne && App.DataContext.isInRealtyCreating)
                {
                    this.selectedMarker = this.OriginMarker;
                    this.draggingNow = true;
                    this.map1.IsEnabled = false;
                }
            }
        }


        #endregion

        private void Submit_Tap(object sender, GestureEventArgs e)
        {
            MessageBox.Show("accepted");

            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            this.map1.Heading = this.map1.Heading + 12;
        }

        private void MinHeading_Click(object sender, EventArgs e)
        {
            this.map1.Heading = this.map1.Heading - 12;
        }

        private void map1_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "87555bfe-031d-45a2-94ba-ec960fd90426";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "AqA8uwlJ0rHF34MD6sXxAgRhmZTuQwGtw-jR0ZN82R2-b4p3m8i-W8aDv-zjP4bo";

        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e) //save click
        {

            //   this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));

        }

        private void Save_Click(object sender, EventArgs e)
        {
            //  if (App.DataContext.isInRealtyCreating)
            {
                var route = new Build.DataLayer.Model.Route();

                route.ActivityType = App.DataContext.CurrentActivity.ActivityType;

                foreach (var marker in MapMarkersList)
                {
                    route.Points.Add(new Points
                                         {
                                             Y = marker.GeoCoordinate.Longitude,
                                             X = marker.GeoCoordinate.Latitude
                                         });
                }
                route.CreatedTime = DateTime.Now;
                route.Duration = routeDuration;
                route.Length = routeLength;
                route.UserName = App.DataContext.CurrentUser.Login;
                var b = route.ActivityType.Image;
                route.ActivityType.Image = null;
                App.DataContext.CurrentActivity.Image = MapToBitMap();
                App.DataContext.CurrentActivity.TimeStamp = DateTime.Now;
                ApiService<Build.DataLayer.Model.Route>.SendPost(route);


                route.ActivityType.Image = b;
                this.NavigationService.Navigate(new Uri("/PageOfChoice.xaml", UriKind.Relative));
            }

            // this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }

        private BitmapImage MapToBitMap()
        {
            var writeableBitmap = new WriteableBitmap((int)map1.RenderSize.Width, (int)map1.RenderSize.Height);

            writeableBitmap.Render(map1, new ScaleTransform() { ScaleX = 1, ScaleY = 1 });
            writeableBitmap.Invalidate();

            //  Image img = new Image();
            // img.Source = writeableBitmap;
            BitmapImage biImg = new BitmapImage();

            using (MemoryStream ms = new MemoryStream())
            {
           Extensions.SaveJpeg(writeableBitmap, ms,
                   (int)map1.RenderSize.Width, (int)map1.RenderSize.Height, 0, 100);
           ms.Seek(0, SeekOrigin.Begin);
                biImg.SetSource(ms);
            }
            return biImg;

        }
        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            var writeableBitmap = new WriteableBitmap((int)map1.RenderSize.Width, (int)map1.RenderSize.Height);

            writeableBitmap.Render(map1, new ScaleTransform() { ScaleX = 1, ScaleY = 1 });
            writeableBitmap.Invalidate();

            //  Image img = new Image();
            // img.Source = writeableBitmap;
            using (MemoryStream ms = new MemoryStream())
            {
                Extensions.SaveJpeg(writeableBitmap, ms,
                   (int)map1.RenderSize.Width, (int)map1.RenderSize.Height, 0, 100);
                ms.Seek(0, SeekOrigin.Begin);
                DataService.SaveImage("Asdas.jpg", ms.ToArray());
            }
            MessageBox.Show("Adac");

        }
    }
}