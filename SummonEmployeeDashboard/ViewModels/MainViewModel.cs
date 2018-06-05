using Newtonsoft.Json;
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

namespace SummonEmployeeDashboard.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public Action CloseAction { get; set; }

        private PeopleViewModel peopleVM;
        public PeopleViewModel PeopleVM
        {
            get { return peopleVM; } set
            {
                peopleVM = value;
                OnPropertyChanged("PeopleVM");
            }
        }

        public MainViewModel(Action closeAction)
        {
            CloseAction = closeAction;
            Initialize();
        }

        private async void Initialize()
        {
            var accessToken = App.GetApp().AccessToken;
            if (accessToken == null)
            {
                Login();
            }
            else
            {
                await PingAsync(accessToken.Id);
            }
        }

        private async Task PingAsync(string accessToken)
        {
            try
            {
                var isValid = await App.GetApp().GetService<PeopleService>().Ping(accessToken);
                if (isValid)
                {
                    PeopleVM = new PeopleViewModel();
                }
                else
                {
                    var loginWindow = new LoginWindow();
                    loginWindow.Show();
                    CloseAction();
                }
            } catch (Exception)
            {
                Login();
            }
        }

        private void Login()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            CloseAction();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
