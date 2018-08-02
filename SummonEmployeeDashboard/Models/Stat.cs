using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SummonEmployeeDashboard.Models
{
    class Stat : INotifyPropertyChanged
    {
        private DateTime? date = null;
        private int incoming;
        private int outgoing;

        public DateTime? Date
        {
            get => date; set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public int Incoming
        {
            get => incoming; set
            {
                incoming = value;
                OnPropertyChanged("Incoming");
            }
        }

        public int Outgoing
        {
            get => outgoing; set
            {
                outgoing = value;
                OnPropertyChanged("Outgoing");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
