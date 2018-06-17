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
            get { return peopleVM; }
            set
            {
                peopleVM = value;
                OnPropertyChanged("PeopleVM");
            }
        }

        private EditPeopleViewModel editPeopleVM;
        public EditPeopleViewModel EditPeopleVM
        {
            get { return editPeopleVM; }
            set
            {
                editPeopleVM = value;
                OnPropertyChanged("EditPeopleVM");
            }
        }

        private RequestsViewModel incomingRequestsVM;
        public RequestsViewModel IncomingRequestsVM
        {
            get { return incomingRequestsVM; }
            set
            {
                incomingRequestsVM = value;
                OnPropertyChanged("IncomingRequestsVM");
            }
        }

        private RequestsViewModel outgoingRequestsVM;
        public RequestsViewModel OutgoingRequestsVM
        {
            get { return outgoingRequestsVM; }
            set
            {
                outgoingRequestsVM = value;
                OnPropertyChanged("OutgoingRequestsVM");
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
            if (accessToken != null)
            {
                var isValid = await PingAsync(accessToken.Id);
                if (isValid)
                {
                    PeopleVM = new PeopleViewModel();
                    IncomingRequestsVM = new RequestsViewModel(true);
                    OutgoingRequestsVM = new RequestsViewModel(false);
                    EditPeopleVM = new EditPeopleViewModel();
                    return;
                }
            }
            Login();
        }

        private async Task<Boolean> PingAsync(string accessToken)
        {
            try
            {
               return await App.GetApp().GetService<PeopleService>().Ping(accessToken);
            } catch (Exception)
            {
            }
            return false;
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
