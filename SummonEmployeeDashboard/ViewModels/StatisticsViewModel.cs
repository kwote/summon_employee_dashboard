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
using System.Windows;
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class StatisticsViewModel : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(StatisticsViewModel));
        private readonly int personId;

        private PersonStatVM selectedStat;
        public PersonStatVM SelectedStat
        {
            get { return selectedStat; }
            set
            {
                selectedStat = value;
                OnPropertyChanged("SelectedStat");
            }
        }
        private ObservableCollection<PersonStatVM> stats;
        public ObservableCollection<PersonStatVM> Stats
        {
            get => stats;
            set {
                stats = value;
                OnPropertyChanged("Stats");
            }
        }
        private DateTime dateFrom;
        public DateTime DateFrom
        {
            get => dateFrom;
            set
            {
                dateFrom = value;
                OnPropertyChanged("DateFrom");
            }
        }
        private DateTime dateTo;
        public DateTime DateTo
        {
            get => dateTo;
            set
            {
                dateTo = value;
                OnPropertyChanged("DateTo");
            }
        }
        private ObservableCollection<RequestType> requestTypes;
        public ObservableCollection<RequestType> RequestTypes
        {
            get => requestTypes;
            set
            {
                requestTypes = value;
                OnPropertyChanged("RequestTypes");
            }
        }
        private RequestType requestType;
        public RequestType RequestType
        {
            get => requestType;
            set
            {
                requestType = value;
                OnPropertyChanged("RequestType");
            }
        }
        public Visibility HeaderVisible
        {
            get { return days.Count > 0 ? Visibility.Visible : Visibility.Collapsed; }
        }
        private string selectedDay;
        public string SelectedDay
        {
            get => selectedDay;
            set
            {
                selectedDay = value;
                OnPropertyChanged("SelectedDay");
            }
        }

        private ObservableCollection<string> days;
        public ObservableCollection<string> Days
        {
            get => days;
            set
            {
                days = value;
                OnPropertyChanged("Days");
                OnPropertyChanged("HeaderVisible");
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

        private readonly SynchronizationContext syncContext;

        public StatisticsViewModel(int personId)
        {
            syncContext = SynchronizationContext.Current;
            this.personId = personId;
            dateFrom = DateTime.Now.AddDays(-7);
            dateTo = DateTime.Now;
            requestTypes = new ObservableCollection<RequestType>(RequestType.Types());
            requestType = RequestType.Incoming();
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
                    var accessToken = app.AccessToken;
                    var from = dateFrom.Date;
                    var to = dateTo.Date;
                    var stats = app.GetService<PeopleService>().GetStatistics(personId, requestType, from, to, accessToken.Id);

                    app.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Stats = new ObservableCollection<PersonStatVM>(stats.ConvertAll(s => new PersonStatVM(s, from, to)));
                        var date = from;
                        var days = new List<string>();
                        while (date <= to)
                        {
                            days.Add(date.ToShortDateString());
                            date = date.AddDays(1);
                        }
                        Days = new ObservableCollection<string>(days);
                    }));
                }
                catch (Exception e)
                {
                    log.Error("Failed to load statistics", e);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
