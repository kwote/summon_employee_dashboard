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
    class PersonWithStatsVM : INotifyPropertyChanged
    {
        private PersonVM person;
        public PersonVM Person
        {
            get { return person; }
            set
            {
                person = value;
                OnPropertyChanged("Person");
            }
        }
        private StatisticsViewModel stats;
        public StatisticsViewModel Stats
        {
            get { return stats; }
            set
            {
                stats = value;
                OnPropertyChanged("Stats");
            }
        }

        public PersonWithStatsVM()
        {
            Initialize();
        }

        private void Initialize()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
