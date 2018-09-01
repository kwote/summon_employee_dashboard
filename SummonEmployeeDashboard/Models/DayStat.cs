using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SummonEmployeeDashboard.Models
{
    class DayStat : INotifyPropertyChanged
    {
        private DateTime? date = null;
        private int accepted;
        private int rejected;
        private int pending;

        public DateTime? Date
        {
            get => date; set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public int Accepted
        {
            get => accepted; set
            {
                accepted = value;
                OnPropertyChanged("Accepted");
            }
        }

        public int Rejected
        {
            get => rejected; set
            {
                rejected = value;
                OnPropertyChanged("Rejected");
            }
        }

        public int Pending
        {
            get => pending; set
            {
                pending = value;
                OnPropertyChanged("Pending");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
