#region Using Directives

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Build.DataLayer.Interfaces;
using Build.DataLayer.Model;

using BuildSeller.Core.Model;

using DragDropPhoneApp.ApiConsumer;
using DragDropPhoneApp.Helpers;

using Microsoft.Phone.Controls;

using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion

namespace DragDropPhoneApp
{
    public partial class LoginPage : PhoneApplicationPage
    {
        #region Fields

        private IRepository<CurrentUser> userRepository = App.UserRepository;

        #endregion

        #region Constructors and Destructors

        public LoginPage()
        {
            this.InitializeComponent();
            this.DataContext = App.DataContext;
        }

        #endregion

        #region Methods

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LoginBtn_Tap(object sender, GestureEventArgs e)
        {
            if (this.Login.Text != string.Empty && this.Password.Text != string.Empty)
            {
                App.DataContext.IsLoading = true;
                ApiService<Users>.Login(this.Login.Text, this.Password.Text);
                var user = new CurrentUser
                               {
                                   Login = this.Login.Text, 
                                   Password = this.Password.Text, 
                                   LoginTime = DateTime.Now
                               };
                this.userRepository.Insert(user);
                App.DataContext.CurrentUser.Login = this.Login.Text;
                this.userRepository.SubmitChanges();
            }
            else
            {
                MessageBox.Show("Username and password fields cannot be empty");
            }
        }

        private void Login_Tap(object sender, GestureEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Indicator.SetLoadingIndicator(this, "Loggin in");
            var user = this.userRepository.GetAll().OrderByDescending(b => b.LoginTime).FirstOrDefault();

            if (user == null)
            {
                return;
            }

            this.Login.Text = user.Login;
            this.Password.Text = user.Password;
            Task.Factory.StartNew(
                () =>
                    {
                        var users = this.userRepository.GetAll().OrderByDescending(b => b.LoginTime).Skip(1);
                        this.userRepository.DeleteAll(users);
                    });
        }

        #endregion
    }
}