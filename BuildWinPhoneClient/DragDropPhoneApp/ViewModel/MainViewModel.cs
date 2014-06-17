#region Using Directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

using Build.DataLayer.Model;

using BuildSeller.Core.Model;

using DragDropPhoneApp.Helpers;

using GalaSoft.MvvmLight;

#endregion

namespace DragDropPhoneApp.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        public Dictionary<int, bool> DownloadImageUnderNumberCompleted = new Dictionary<int, bool>();

        private bool isLoading;


        private List<Activity> activities;

        private List<Route> routes;

        #endregion

        #region Constructors and Destructors

        public MainViewModel()
        {
            this.Routes = new List<Route>();
            this.CurrentUser = new Users { Login = "Login", Password = "Pass", };
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public Activity CurrentActivity { get; set; }

        public Route CurrentRoute { get; set; }

        public Users CurrentUser { get; set; }

        public List<KeyedList<string, Activity>> GroupedActivities
        {
            get
            {
                if (this.Activities == null)
                {
                    return null;
                }

                var groupedPhotos = from photo in this.Activities
                                    orderby photo.TimeStamp
                                    group photo by photo.TimeStamp.ToString("y")
                                    into images
                                    select new KeyedList<string, Activity>(images);

                return new List<KeyedList<string, Activity>>(groupedPhotos);
            }
        }

    

        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                this.NotifyPropertyChanged();
            }
        }

    
        public List<Route> Routes
        {
            get
            {
                return this.routes;
            }

            set
            {
                this.routes = value;
                Deployment.Current.Dispatcher.BeginInvoke(() => { this.NotifyPropertyChanged("UserRoutesList"); });
            }
        }

        public List<AlphaKeyGroup<Route>> UserRoutesList
        {
            get
            {
                var cards = this.Routes;
                return AlphaKeyGroup<Route>.CreateGroups(cards, s => s.UserName, true);
            }
        }

        public List<Activity> Activities
        {
            get
            {
                return this.activities;
            }

            set
            {
                this.activities = value;
                this.NotifyPropertyChanged("GroupedActivities");
            }
        }

        #endregion

        #region Public Methods and Operators

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}