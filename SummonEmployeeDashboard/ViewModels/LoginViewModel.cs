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
                async param => await Login(),
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
            ServerIP = App.GetApp().ServerIP;
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

        private async Task Login()
        {
            try
            {
                App app = App.GetApp();
                app.ServerIP = ServerIP;
                var accessToken = await app.GetService<PeopleService>().Login(credentials);
                if (accessToken != null)
                {
                    App.GetApp().AccessToken = accessToken;
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    CloseAction();
                }
            } catch (Exception e)
            {
                Error = e.Message;
            }
        }

        private void Register()
        {
            App app = App.GetApp();
            app.ServerIP = ServerIP;
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            CloseAction();
        }

        private string serverIP = "";
        public string ServerIP
        {
            get
            {
                return serverIP;
            }
            set
            {
                serverIP = value;
                OnPropertyChanged("ServerIP");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
