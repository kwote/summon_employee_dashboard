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
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class RequestsViewModel : INotifyPropertyChanged
    {
        private SummonRequest selectedRequest;
        public SummonRequest SelectedRequest
        {
            get { return selectedRequest; }
            set
            {
                selectedRequest = value;
                OnPropertyChanged("SelectedRequest");
            }
        }

        private ObservableCollection<SummonRequest> requests;
        public ObservableCollection<SummonRequest> Requests
        {
            get {
                return requests;
            }
            set {
                requests = value;
                OnPropertyChanged("Requests");
            }
        }

        private ICommand reloadCommand;

        public ICommand ReloadCommand
        {
            get
            {
                if (reloadCommand == null)
                {
                    reloadCommand = new RelayCommand(
                        param => Reload(),
                        param => CanReload()
                    );
                }
                return reloadCommand;
            }
        }

        private bool CanReload()
        {
            return true;
        }

        private ICommand acceptCommand;

        public ICommand AcceptCommand
        {
            get
            {
                if (acceptCommand == null)
                {
                    acceptCommand = new RelayCommand(
                        param => Accept(),
                        param => CanAccept()
                    );
                }
                return acceptCommand;
            }
        }

        private async void Accept()
        {
            AccessToken accessToken = App.GetApp().AccessToken;
            await App.GetApp().GetService<SummonRequestService>()
                .Accept(SelectedRequest.Id, accessToken.Id);
        }

        private bool CanAccept()
        {
            return true;
        }

        private ICommand rejectCommand;

        public ICommand RejectCommand
        {
            get
            {
                if (rejectCommand == null)
                {
                    rejectCommand = new RelayCommand(
                        param => Reject(),
                        param => CanReject()
                    );
                }
                return rejectCommand;
            }
        }

        private async void Reject()
        {
            AccessToken accessToken = App.GetApp().AccessToken;
            await App.GetApp().GetService<SummonRequestService>()
                .Reject(SelectedRequest.Id, accessToken.Id);
        }

        private bool CanReject()
        {
            return true;
        }

        public bool Incoming { get; set; }

        public RequestsViewModel(bool incoming)
        {
            Incoming = incoming;
            Initialize();
        }

        private void Initialize()
        {
            Reload();
        }

        private async void Reload()
        {
            AccessToken accessToken = App.GetApp().AccessToken;
            if (Incoming)
            {
                Requests = new ObservableCollection<SummonRequest>(
                    await App.GetApp().GetService<PeopleService>().ListIncomingRequests(accessToken.UserId, accessToken.Id));
            }
            else
            {
                Requests = new ObservableCollection<SummonRequest>(
                    await App.GetApp().GetService<PeopleService>().ListOutgoingRequests(accessToken.UserId, accessToken.Id));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
