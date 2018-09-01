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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class StatisticsViewModel : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(StatisticsViewModel));
        private readonly int personId;

        private ObservableCollection<PersonStatVM> stats;
        public ObservableCollection<PersonStatVM> Stats {
            get => stats;
            set
            {
                stats = value;
                OnPropertyChanged("Stats");
                OnPropertyChanged("SelfVisibility");
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
        private RequestType selectedRequestType;
        public RequestType SelectedRequestType
        {
            get => selectedRequestType;
            set
            {
                selectedRequestType = value;
                OnPropertyChanged("SelectedRequestType");
            }
        }
        private ObservableCollection<DataGridColumn> columns;
        public ObservableCollection<DataGridColumn> Columns {
            get => columns;
            set
            {
                columns = value;
                OnPropertyChanged("Columns");
            }
        }

        public Visibility SelfVisibility
        {
            get { return stats != null ? Visibility.Visible : Visibility.Hidden; }
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
            selectedRequestType = RequestType.Incoming();
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
                App app = App.GetApp();
                var accessToken = app.AccessToken;
                var from = dateFrom.Date;
                var to = dateTo.Date;
                var stats = await app.GetService<PeopleService>().GetStatistics(personId, selectedRequestType, from, to, accessToken.Id);

                Stats = new ObservableCollection<PersonStatVM>(stats.ConvertAll(s => new PersonStatVM(s, from, to)));
                var date = from;
                var days = new List<DateTime>();
                while (date <= to)
                {
                    days.Add(date);
                    date = date.AddDays(1);
                }
                Columns = BuildColumns(days);
            }
            catch (Exception e)
            {
                log.Error("Failed to load statistics", e);
            }
        }

        private ObservableCollection<DataGridColumn> BuildColumns(List<DateTime> days)
        {
            var columns = new List<DataGridColumn>();
            var binding = new Binding("PersonName");
            columns.Add(new DataGridTextColumn() { Header = "Имя", Binding = binding });
            int i = 0;
            foreach (DateTime day in days)
            {
                var name = day.ToShortDateString();
                binding = new Binding(string.Format("DayStats[{0}]", i++));
                columns.Add(new CustomBoundColumn() { Header = name, Binding = binding, TemplateName = "StatTemplate" });
            }
            return new ObservableCollection<DataGridColumn>(columns);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
