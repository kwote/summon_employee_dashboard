﻿using Newtonsoft.Json;
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
using System.Windows;
using System.Windows.Input;

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

        private readonly SynchronizationContext syncContext;

        public MainViewModel()
        {
            Initialize();
        }

        public MainViewModel(Action close)
        {
            CloseAction = close;
            Initialize();
        }
        private AccessToken accessToken;

        private Timer pingTimer;

        private void Initialize()
        {
            Task.Factory.StartNew(() =>
            {
                App app = App.GetApp();
                accessToken = app.AccessToken;
                if (accessToken != null)
                {
                    var isValid = Ping(accessToken.Id);
                    if (isValid)
                    {
                        var role = app.GetService<PeopleService>().GetRole(accessToken.User.Id, accessToken.Id);
                        app.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ReloadPeople();
                            ReloadEditPeople();
                            ReloadRequests(true);
                            ReloadRequests(false);
                            ReloadStatistics();
                            ReloadPeopleStatistics();
                            try
                            {
                                Role = role;
                            } catch (Exception)
                            {
                            }
                        }));

                        var autoEvent = new AutoResetEvent(false);
                        pingTimer = new Timer((o) =>
                        {
                            Task.Factory.StartNew(() =>
                            {
                                var valid = Ping(accessToken.Id);
                                if (!valid)
                                {
                                    pingTimer.Dispose();
                                    app.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        Login();
                                    }));
                                }
                            });
                        }, autoEvent, App.PING_PERIOD * 1000, App.PING_PERIOD * 1000);
                        return;
                    }
                }
                app.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Login();
                }));
            });
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

        private void Logout()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    App app = App.GetApp();
                    app.GetService<PeopleService>().Logout(app.AccessToken.Id);
                    pingTimer?.Dispose();
                    app.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Login();
                    }));
                }
                catch (Exception)
                {
                }
            });
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

        private bool Ping(string accessToken)
        {
            try
            {
               return App.GetApp().GetService<PeopleService>().Ping(accessToken);
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
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
