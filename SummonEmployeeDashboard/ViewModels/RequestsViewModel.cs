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
        private SummonRequestVM selectedRequest;
        public SummonRequestVM SelectedRequest
        {
            get { return selectedRequest; }
            set
            {
                selectedRequest = value;
                OnPropertyChanged("SelectedRequest");
            }
        }

        private ObservableCollection<SummonRequestVM> requests;
        public ObservableCollection<SummonRequestVM> Requests
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
            try
            {
                AccessToken accessToken = App.GetApp().AccessToken;
                SelectedRequest = new SummonRequestVM(Incoming);
                var requests = Incoming
                        ? await App.GetApp().GetService<PeopleService>().ListIncomingRequests(accessToken.UserId, accessToken.Id)
                        : await App.GetApp().GetService<PeopleService>().ListOutgoingRequests(accessToken.UserId, accessToken.Id)
                ;
                Requests = new ObservableCollection<SummonRequestVM>(requests.ConvertAll(r => new SummonRequestVM(Incoming) { Request = r }));
            }
            catch (Exception)
            {
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
