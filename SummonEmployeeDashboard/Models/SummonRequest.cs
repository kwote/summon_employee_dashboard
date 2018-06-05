using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Models
{
    enum RequestStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }
    class SummonRequest : INotifyPropertyChanged
    {
        private int? id;
        private int callerId;
        private int targetId;
        private string requested;
        private string responded;
        private RequestStatus state;
        private bool enabled;

        public int? Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public int CallerId
        {
            get => callerId; set
            {
                callerId = value;
                OnPropertyChanged("CallerId");
            }
        }
        public int TargetId
        {
            get => targetId; set
            {
                targetId = value;
                OnPropertyChanged("TargetId");
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
        public RequestStatus State
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
