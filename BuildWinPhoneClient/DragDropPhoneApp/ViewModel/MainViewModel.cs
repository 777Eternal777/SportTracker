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
        private List<Photo> phots;
        public Users CurrentUser { get; set; }

        public List<Realty> realtys;

        private bool isLoading;
        public bool isInRealtyCreating;
        public List<Photo> photos
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

        #endregion

        #region Constructors and Destructors

        public MainViewModel()
        {
            this.Realtys = new List<Realty>();
            this.CurrentUser = new Users
                                   {
                                       Login = "Login",
                                       Password = "Pass",
                                       Phone = "Phone",
                                       Adress = "Address",
                                       FirstName = "FirstName",
                                       LastName = "LastName",
                                       Email = "Emal@asd.ru",
                                       Patronymic = "Patronymic",
                                       
                                       
                                   };
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public Realty CurrentRealty { get; set; }

        public List<AlphaKeyGroup<Realty>> GroupedRealtiesForRent
        {
            get
            {
                var cards = this.Realtys.Where(v => v.IsForRent);
                return AlphaKeyGroup<Realty>.CreateGroups(cards, s => s.Named, true);
            }
        }

        public List<AlphaKeyGroup<Realty>> GroupedRealtiesForSell
        {
            get
            {
                var cards = this.Realtys.Where(v => !v.IsForRent);
                return AlphaKeyGroup<Realty>.CreateGroups(cards, s => s.Named, true);
            }
        }

        public List<KeyedList<string, Photo>> GroupedPhotos
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
                                        select new KeyedList<string, Photo>(images);

                return new List<KeyedList<string, Photo>>(groupedPhotos);
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

        public List<Realty> Realtys
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
        }
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