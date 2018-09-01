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
    class DayStatVM : INotifyPropertyChanged
    {
        private DayStat stat;

        public DayStatVM(DayStat stat)
        {
            this.stat = stat;
            
            Initialize();
        }

        public string Date
        {
            get
            {
                return Stat.Date?.ToShortDateString();
            }
        }

        public string Accepted
        {
            get
            {
                return "Принятые: " + Stat.Accepted;
            }
        }
        
        public string Rejected
        {
            get
            {
                return "Отклонённые: " + Stat.Rejected;
            }
        }

        public string Pending
        {
            get
            {
                return "Пропущенные: " + Stat.Pending;
            }
        }

        public Visibility SelfVisibility
        {
            get { return stat != null ? Visibility.Visible : Visibility.Hidden; }
        }

        internal DayStat Stat { get => stat;
            set
            {
                stat = value;
                OnPropertyChanged("Stat");
            }
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
