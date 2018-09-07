using SummonEmployeeDashboard.Models;
using SummonEmployeeDashboard.Rest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoginViewModel));
        private LoginCredentials credentials;
        public LoginCredentials Credentials
        {
            get { return credentials; }
            set
            {
                credentials = value;
                OnPropertyChanged("Credentials");
            }
        }

        private string error = "";
        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                OnPropertyChanged("Error");
            }
        }

        public Action CloseAction { get; set; }
        public string Password { get => Credentials.Password; set => Credentials.Password = value; }

        public LoginViewModel(Action action)
        {
            CloseAction = action;
            LoginCommand = new RelayCommand(
                param => Login(),
                param => CanLogin()
            );
            RegisterCommand = new RelayCommand(
                param => Register(),
                param => true
            );
            Initialize();
        }

        private void Initialize()
        {
            credentials = new LoginCredentials();
            ServerURL = App.GetApp().ServerURL;
        }

        public ICommand LoginCommand { get; }

        public ICommand RegisterCommand { get; }

        private bool CanLogin()
        {
            if (!credentials.Email.Contains('@'))
            {
                return false;
            }
            if (credentials.Password.Length < 5)
            {
                return false;
            }
            return true;
        }

        private bool loggingIn = false;

        private void Login()
        {
            if (loggingIn) return;
            App app = App.GetApp();
            app.ServerURL = ServerURL;
            Error = "";
            Task.Factory.StartNew(() =>
            {
                loggingIn = true;
                try
                {
                    var accessToken = app.GetService<PeopleService>().Login(credentials);
                    if (accessToken != null)
                    {
                        app.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            app.AccessToken = accessToken;
                            var mainWindow = new MainWindow();
                            mainWindow.Show();
                            CloseAction();
                        }));
                    }
                } catch (Exception e)
                {
                    log.Error("Failed to login", e);
                    app.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Error = "Не удалось войти";
                    }));
                }
                loggingIn = false;
            });
        }

        private void Register()
        {
            Error = "";
            App app = App.GetApp();
            app.ServerURL = ServerURL;
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            CloseAction();
        }

        private string serverURL = "";
        public string ServerURL
        {
            get
            {
                return serverURL;
            }
            set
            {
                serverURL = value;
                OnPropertyChanged("ServerURL");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
