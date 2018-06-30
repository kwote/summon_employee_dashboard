using EvtSource;
using Newtonsoft.Json;
using SummonEmployeeDashboard.Models;
using SummonEmployeeDashboard.Rest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.ViewModels
{
    class MainViewModel : INotifyPropertyChanged, IObserver<SummonRequestUpdate>
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

        private readonly SynchronizationContext syncContext;

        public MainViewModel()
        {
            syncContext = SynchronizationContext.Current;
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
                    ReloadPeople();
                    ReloadEditPeople();
                    ReloadRequests(true);
                    ReloadRequests(false);
                    App.GetApp().EventBus.Subscribe(this);
                    App.GetApp().EventBus.Initialize(accessToken);
                    return;
                }
            }
            Login();
        }

        private void ReloadPeople()
        {
            PeopleVM = new PeopleViewModel();
        }

        private void ReloadEditPeople()
        {
            EditPeopleVM = new EditPeopleViewModel();
        }

        private void ReloadRequests(bool incoming)
        {
            if (incoming) IncomingRequestsVM = new RequestsViewModel(true);
            else
                OutgoingRequestsVM = new RequestsViewModel(false);
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

        public void OnNext(SummonRequestUpdate update)
        {
            switch (update.UpdateType)
            {
                case UpdateType.Create:
                    syncContext.Post(o =>
                    {
                        var incomingRequestWindow = new SummonRequestWindow(update.Request);
                        incomingRequestWindow.Show();
                    }, null);
                    break;
                case UpdateType.Cancel:
                    break;
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
