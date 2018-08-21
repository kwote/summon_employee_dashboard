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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class RequestsViewModel : INotifyPropertyChanged, IObserver<SummonRequestUpdate>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(RequestsViewModel));
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

        private readonly SynchronizationContext syncContext;

        public RequestsViewModel(bool incoming)
        {
            syncContext = SynchronizationContext.Current;
            Incoming = incoming;
            Initialize();
        }

        private void Initialize()
        {
            Reload();
        }

        private void Reload()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    App app = App.GetApp();
                    AccessToken accessToken = app.AccessToken;
                    var requests = Incoming
                            ? app.GetService<PeopleService>().ListIncomingRequests(accessToken.UserId, accessToken.Id)
                            : app.GetService<PeopleService>().ListOutgoingRequests(accessToken.UserId, accessToken.Id)
                    ;
                    app.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        SelectedRequest = new SummonRequestVM(Incoming);
                        Requests = new ObservableCollection<SummonRequestVM>(
                            requests.ConvertAll(r => new SummonRequestVM(Incoming) { Request = r })
                        );
                    }));
                }
                catch (Exception e)
                {
                    log.Error("Failed to load " + (Incoming ? "incoming" : "outgoing") + " requests", e);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void OnNext(SummonRequestUpdate update)
        {
            switch (update.UpdateType)
            {
                case UpdateType.Accept:
                    syncContext.Post(o =>
                    {
                    }, null);
                    break;
                case UpdateType.Reject:
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
