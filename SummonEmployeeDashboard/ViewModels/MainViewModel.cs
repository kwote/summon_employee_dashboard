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
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class MainViewModel : INotifyPropertyChanged, IObserver<SummonRequestUpdate>, IObserver<string>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MainViewModel));
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

        private StatisticsViewModel statisticsVM;
        public StatisticsViewModel StatisticsVM
        {
            get { return statisticsVM; }
            set
            {
                statisticsVM = value;
                OnPropertyChanged("StatisticsVM");
            }
        }

        private PeopleStatsViewModel peopleStatsVM;
        public PeopleStatsViewModel PeopleStatsVM
        {
            get { return peopleStatsVM; }
            set
            {
                peopleStatsVM = value;
                OnPropertyChanged("PeopleStatsVM");
            }
        }

        private Role role = null;
        public Role Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged("AdminVisible");
            }
        }
        public Visibility AdminVisible
        {
            get { return role?.Name == "admin" ? Visibility.Visible : Visibility.Collapsed; }
        }
        private bool active = false;
        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                OnPropertyChanged("Active");
            }
        }

        private readonly SynchronizationContext syncContext;

        public MainViewModel()
        {
            syncContext = SynchronizationContext.Current;
            Initialize();
        }

        public MainViewModel(Action close)
        {
            CloseAction = close;
            syncContext = SynchronizationContext.Current;
            Initialize();
        }
        private AccessToken accessToken;
        private IDisposable requestSubscription;
        private IDisposable messageSubscription;

        private async void Initialize()
        {
            App app = App.GetApp();
            accessToken = app.AccessToken;
            if (accessToken != null)
            {
                var isValid = await Ping(accessToken.Id, app);
                if (isValid)
                {
                    Active = true;
                    ReloadPeople();
                    ReloadEditPeople();
                    ReloadRequests(true);
                    ReloadRequests(false);
                    ReloadStatistics();
                    ReloadPeopleStatistics();
                    requestSubscription = app.EventBus.Subscribe(this);
                    messageSubscription = app.EventBus.SubscribeToMessage(this);
                    app.EventBus.Initialize(accessToken);
                    try
                    {
                        Role = await app.GetService<PeopleService>().GetRole(accessToken.User.Id, accessToken.Id);
                    } catch (Exception e)
                    {
                        log.Error("Failed to get role", e);
                    }
                    return;
                }
            }
            Login();
        }

        private ICommand logoutCommand;

        public ICommand LogoutCommand
        {
            get
            {
                if (logoutCommand == null)
                {
                    logoutCommand = new RelayCommand(
                        param => Logout(),
                        param => CanLogout()
                    );
                }
                return logoutCommand;
            }
        }

        private async void Logout()
        {
            if (!Active) return;
            Active = false;
            App app = App.GetApp();
            try
            {
                await app.GetService<PeopleService>().Logout(app.AccessToken.Id);
            }
            catch (Exception e)
            {
                log.Error("Failed to logout", e);
            }
            app.EventBus.Dispose();
            Login();
        }

        private bool CanLogout()
        {
            return true;
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
            if (incoming)
                IncomingRequestsVM = new RequestsViewModel(true);
            else
                OutgoingRequestsVM = new RequestsViewModel(false);
        }

        private void ReloadStatistics()
        {
            App app = App.GetApp();
            var accessToken = app.AccessToken;
            StatisticsVM = new StatisticsViewModel(accessToken.UserId);
        }

        private void ReloadPeopleStatistics()
        {
            PeopleStatsVM = new PeopleStatsViewModel();
        }

        private async Task<bool> Ping(string accessToken, App app)
        {
            try
            {
               return await app.GetService<PeopleService>().Ping(accessToken);
            } catch (Exception e)
            {
                log.Error("Ping failed", e);
            }
            return false;
        }

        private void Login()
        {
            requestSubscription?.Dispose();
            messageSubscription?.Dispose();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            CloseAction();
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
                        incomingRequestWindow.WindowState = WindowState.Normal;
                        incomingRequestWindow.ShowActivated = false;
                        incomingRequestWindow.Topmost = true;
                        //window.Focus();
                        incomingRequestWindow.Activate();
                    }, null);
                    break;
                case UpdateType.Cancel:
                    break;
            }
        }

        public void OnNext(string value)
        {
            switch (value)
            {
                case EventBus.DISCONNECTED:
                    syncContext.Post(o =>
                    {
                        Logout();
                    }, null);
                    break;
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
