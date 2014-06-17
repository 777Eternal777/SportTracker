#region Using Directives

using Build.DataLayer.Model;
using Build.DataLayer.Repository;

using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace DragDropPhoneApp.ViewModel
{
    public class ViewModelLocator
    {
        #region Constructors and Destructors

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<Repository<CurrentUser>>();
        }

        #endregion

        #region Public Properties

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public Repository<CurrentUser> UserRepository
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Repository<CurrentUser>>();
            }
        }

        #endregion
    }
}