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
    class StatVM : INotifyPropertyChanged
    {
        private Stat stat;
        public Stat Stat
        {
            get { return stat; }
            set
            {
                stat = value;
                OnPropertyChanged("Stat");
            }
        }

        public string Incoming
        {
            get
            {
                return "Входящие: " + Stat.Incoming;
            }
        }

        public string Outgoing
        {
            get
            {
                return "Исходящие: " + Stat.Outgoing;
            }
        }

        public Visibility SelfVisibility
        {
            get { return stat != null ? Visibility.Visible : Visibility.Hidden; }
        }

        public StatVM()
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
