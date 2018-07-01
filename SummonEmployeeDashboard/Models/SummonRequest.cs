using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Models
{
    public enum RequestState
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }
    public class SummonRequest : INotifyPropertyChanged
    {
        private int id;
        private DateTime? requested;
        private DateTime? responded;
        private RequestState state;
        private bool enabled;
        private Person caller;
        private Person target;

        [JsonProperty(PropertyName = "id")]
        public int Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        [JsonProperty(PropertyName = "callerId")]
        public int CallerId { get; set; }

        [JsonProperty(PropertyName = "caller")]
        public Person Caller { get => caller; set
            {
                caller = value;
                OnPropertyChanged("Caller");
            }
        }
        [JsonProperty(PropertyName = "targetId")]
        public int TargetId { get; set; }

        [JsonProperty(PropertyName = "target")]
        public Person Target
        {
            get => target; set
            {
                target = value;
                OnPropertyChanged("Target");
            }
        }
        [JsonProperty(PropertyName = "requested")]
        public DateTime? Requested
        {
            get => requested; set
            {
                requested = value;
                OnPropertyChanged("Requested");
            }
        }
        [JsonProperty(PropertyName = "responded")]
        public DateTime? Responded
        {
            get => responded; set
            {
                responded = value;
                OnPropertyChanged("Responded");
            }
        }
        [JsonProperty(PropertyName = "state")]
        public RequestState State
        {
            get => state; set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }
        [JsonProperty(PropertyName = "enabled")]
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
