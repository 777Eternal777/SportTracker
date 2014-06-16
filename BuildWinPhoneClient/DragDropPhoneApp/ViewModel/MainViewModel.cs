namespace DragDropPhoneApp.ViewModel
{
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

    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields
        private List<Activity> phots;
        public Users CurrentUser { get; set; }

       // public List<Realty> realtys;

        private bool isLoading;
        public bool isInRealtyCreating;
        public List<Activity> photos
        {
            get
            {
                return this.phots;
            }

            set
            {
                this.phots = value;
                this.NotifyPropertyChanged("GroupedPhotos");
            }
        }
        private List<Route> routes;
        public List<Route> Routes
        {
            get
            {
                return this.routes;
            }

            set
            {
                this.routes = value;
                Deployment.Current.Dispatcher.BeginInvoke(
                    () => { this.NotifyPropertyChanged("UserRoutesList"); });
            }
        }

        #endregion

        #region Constructors and Destructors

        public MainViewModel()
        {
       //     this.Realtys = new List<Realty>();
            Routes = new List<Route>();
            this.CurrentUser = new Users
                                   {
                                       Login = "Login",
                                       Password = "Pass",
                                      
                                       
                                   };
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

     //   public Realty CurrentRealty { get; set; }

        public Route CurrentRoute { get; set; }
        public Activity CurrentActivity { get; set; }
        public List<AlphaKeyGroup<Route>> UserRoutesList
        {
            get
            {
                var cards = this.Routes;
                return AlphaKeyGroup<Route>.CreateGroups(cards, s => s.UserName, true);
            }
        }

       

        public List<KeyedList<string, Activity>> GroupedPhotos
        {
            get
            {
                if (this.photos == null)
                {
                    return null;
                }

                var groupedPhotos = from photo in this.photos
                                    orderby photo.TimeStamp
                                    group photo by photo.TimeStamp.ToString("y")
                                        into images
                                        select new KeyedList<string, Activity>(images);

                return new List<KeyedList<string, Activity>>(groupedPhotos);
            }
        }
        public bool OrderBy
        {
            get
            {
                return this.orderByPrice;
            }

            set
            {
                if (this.orderByPrice != value)
                {
                    this.orderByPrice = value;
                            this.NotifyPropertyChanged("GroupedRealtiesForRent");
                            this.NotifyPropertyChanged("GroupedRealtiesForSell");
                }
            }
        }

        private bool isAscendingSorting;
        public bool IsAscendingSorting
        {
            get
            {
                return this.isAscendingSorting;
            }

            set
            {
                if (this.isAscendingSorting != value)
                {
                    this.isAscendingSorting = value;
                    this.NotifyPropertyChanged("GroupedRealtiesForRent");
                    this.NotifyPropertyChanged("GroupedRealtiesForSell");
                }
            }
        }

        private bool orderByPrice;
        public bool IsAuthorized { get; set; }

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

     /*   public List<Realty> Realtys
        {
            get
            {
                return orderByPrice
                           ? (isAscendingSorting
                                  ? realtys.OrderBy(b => b.Price).ToList()
                                  : realtys.OrderByDescending(b => b.Price).ToList())
                           : (isAscendingSorting
                                  ? realtys.OrderBy(b => b.Square).ToList()
                                  : realtys.OrderByDescending(b => b.Square).ToList());
            }

            set
            {
                this.realtys = value;

                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                        {
                            this.NotifyPropertyChanged("GroupedRealtiesForRent");
                            this.NotifyPropertyChanged("GroupedRealtiesForSell");
                        });
            }
        }*/
        
        public Dictionary<int, bool> DownloadImageUnderNumberCompleted = new Dictionary<int, bool>(); 

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