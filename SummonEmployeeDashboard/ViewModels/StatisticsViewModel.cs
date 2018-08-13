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
    class StatisticsViewModel : INotifyPropertyChanged
    {
        private readonly int personId;
        private StatVM selectedStat;
        public StatVM SelectedStat
        {
            get { return selectedStat; }
            set
            {
                selectedStat = value;
                OnPropertyChanged("SelectedStat");
            }
        }

        private ObservableCollection<StatVM> stats;
        public ObservableCollection<StatVM> Stats
        {
            get {
                return stats;
            }
            set {
                stats = value;
                OnPropertyChanged("Stats");
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
                    var stats = app.GetService<PeopleService>().GetStatistics(personId, accessToken.Id);
                    app.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        SelectedStat = new StatVM();
                        Stats = new ObservableCollection<StatVM>(stats.ConvertAll(s => new StatVM() { Stat = s }));
                    }));
                }
                catch (Exception)
                {
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
