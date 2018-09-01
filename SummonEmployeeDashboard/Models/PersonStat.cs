using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SummonEmployeeDashboard.Models
{
    class PersonStat : INotifyPropertyChanged
    {
        private PersonName person;
        private List<DayStat> stats;

        public PersonName Person
        {
            get => person; set
            {
                person = value;
                OnPropertyChanged("Person");
            }
        }

        public List<DayStat> Stats
        {
            get => stats; set
            {
                stats = value;
                OnPropertyChanged("Stats");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
