using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Models
{
    enum RequestState
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }
    class SummonRequest : INotifyPropertyChanged
    {
        private int id;
        private string requested;
        private string responded;
        private RequestState state;
        private bool enabled;
        private Person caller;
        private Person target;

        public int Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public int CallerId { get; set; }
        public Person Caller { get => caller; set
            {
                caller = value;
                OnPropertyChanged("Caller");
            }
        }
        public int TargetId { get; set; }
        public Person Target
        {
            get => target; set
            {
                target = value;
                OnPropertyChanged("Target");
            }
        }
        public string Requested
        {
            get => requested; set
            {
                requested = value;
                OnPropertyChanged("Requested");
            }
        }
        public string Responded
        {
            get => responded; set
            {
                responded = value;
                OnPropertyChanged("Responded");
            }
        }
        public RequestState State
        {
            get => state; set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }
        public bool Enabled
        {
            get => enabled; set
            {
                enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
