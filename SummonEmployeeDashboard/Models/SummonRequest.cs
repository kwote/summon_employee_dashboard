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
        private string requestTime;
        private string responseTime;
        private RequestStatus status;
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
        public string RequestTime
        {
            get => requestTime; set
            {
                requestTime = value;
                OnPropertyChanged("RequestTime");
            }
        }
        public string ResponseTime
        {
            get => responseTime; set
            {
                responseTime = value;
                OnPropertyChanged("ResponseTime");
            }
        }
        public RequestStatus Status
        {
            get => status; set
            {
                status = value;
                OnPropertyChanged("Status");
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
