﻿using System;
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
    using System.IO.IsolatedStorage;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Windows.Devices.Geolocation;

    using DragDropPhoneApp.ViewModel;

    using Microsoft.Phone.Maps.Controls;
    using Microsoft.Phone.Maps.Services;

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

            //   this.AddResultToMap(
            //    new GeoCoordinate(App.DataContext.CurrentRealty.MapPosX, App.DataContext.CurrentRealty.MapPosY),
            //    new GeoCoordinate(App.DataContext.CurrentRealty.MapPosX + 1, App.DataContext.CurrentRealty.MapPosY + 1));

            this.geoRev = new ReverseGeocodeQuery();
            this.geoRev.QueryCompleted += this.geoRev_QueryCompleted;

            this.geoQ = new RouteQuery();
            this.geoQ.QueryCompleted += this.geoQ_QueryCompleted;
            this.markerLayer = new MapLayer();

            this.map1.Layers.Add(this.markerLayer);
            //   StartGeoLoc();
            if (App.DataContext.isInRealtyCreating)
            {
                //   this.GetRouteBtn.Visibility = Visibility.Collapsed;
                //   this.Submit.Visibility = Visibility.Visible;
            }
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
            this.geoQ.QueryAsync();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (sender == this.GetRouteBtn)
            {
                StartGeoQ();
            }
        }

        private MapOverlay MakeDotMarker(GeoCoordinate location, bool isDestination)
        {
            MapOverlay marker = new MapOverlay();

            marker.GeoCoordinate = location;

            Ellipse circle = new Ellipse();
            if (isDestination)
            {
                circle.Fill = new SolidColorBrush(Colors.Green);
                circle.Stroke = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                circle.Fill = new SolidColorBrush(Colors.Yellow);
                circle.Stroke = new SolidColorBrush(Colors.Red);
            }
            Grid grid = new Grid
                            {
                                Width = 60,
                                Height = 60
                            };
            TextBlock num = new TextBlock
                                {
                                    VerticalAlignment = VerticalAlignment.Center,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    Text = "1"
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

            TouchPoint tPoint = e.GetPrimaryTouchPoint(this.map1);

            if (tPoint.Action == TouchAction.Down)
            {
                var marker = MakeDotMarker(this.map1.ConvertViewportPointToGeoCoordinate(tPoint.Position), false);

                //this.selectedMarker.GeoCoordinate = this.map1.ConvertViewportPointToGeoCoordinate(tPoint.Position);
                this.Start_ReverceGeoCoding(this.selectedMarker);
                StartGeoQ();
            }


            /*       if (!App.DataContext.isInRealtyCreating)
                   {
                       return;
                   }

                   if (this.draggingNow)
                   {
                       TouchPoint tPoint = e.GetPrimaryTouchPoint(this.map1);

                       if (tPoint.Action == TouchAction.Move && (this.selectedMarker != null))
                       {
                           this.selectedMarker.GeoCoordinate = this.map1.ConvertViewportPointToGeoCoordinate(tPoint.Position);
                           this.Start_ReverceGeoCoding(this.selectedMarker);
                       }
                       else if (tPoint.Action == TouchAction.Up)
                       {
                           this.selectedMarker = null;
                           this.draggingNow = false;
                           this.map1.IsEnabled = true;
                       }
                   }*/
        }

        private int failedQueriesCount = 0;

        private bool allfailed;
        private void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (allfailed)
            {
                this.MapMarkersList.RemoveAt(MapMarkersList.Count-1);
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

                MessageBox.Show(
                    "Distance: " + (myRoute.LengthInMeters / 1000) + " km, Estimated traveltime: "
                    + myRoute.EstimatedDuration);
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
            App.DataContext.CurrentRealty.MapPosX = this.OriginMarker.GeoCoordinate.Latitude;
            App.DataContext.CurrentRealty.MapPosY = this.OriginMarker.GeoCoordinate.Longitude;
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

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {

            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));

        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (App.DataContext.isInRealtyCreating)
            {
                App.DataContext.CurrentRealty.MapPosX = this.OriginMarker.GeoCoordinate.Latitude;
                App.DataContext.CurrentRealty.MapPosY = this.OriginMarker.GeoCoordinate.Longitude;
                MessageBox.Show("accepted");
            }

            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }
    }
}