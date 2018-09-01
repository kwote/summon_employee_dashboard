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
using System.Windows;
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class PersonStatVM : INotifyPropertyChanged
    {
        private readonly PersonStat stat;
        private ObservableCollection<DayStatVM> dayStats;
        public ObservableCollection<DayStatVM> DayStats { get => dayStats; set
            {
                dayStats = value;
                OnPropertyChanged("DayStats");
            }
        }

        public PersonStatVM(PersonStat stat, DateTime from, DateTime to)
        {
            this.stat = stat;
            Initialize(from, to);
        }

        public string PersonName
        {
            get => stat.Person.FullName;
        }

        public Visibility SelfVisibility
        {
            get { return stat != null ? Visibility.Visible : Visibility.Hidden; }
        }

        private void Initialize(DateTime from, DateTime to)
        {
            var dayStats = new List<DayStatVM>();
            DateTime date = from;
            var enumerator = stat.Stats.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var dayStat = enumerator.Current;
                var day = dayStat.Date?.Date;
                while (date < day)
                {
                    dayStats.Add(new DayStatVM(new DayStat() { Date = date }));
                    date = date.AddDays(1);
                }
                dayStats.Add(new DayStatVM(dayStat));
                date = date.AddDays(1);
            }
            while (date <= to)
            {
                dayStats.Add(new DayStatVM(new DayStat() { Date = date }));
                date = date.AddDays(1);
            }
            DayStats = new ObservableCollection<DayStatVM>(dayStats);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
