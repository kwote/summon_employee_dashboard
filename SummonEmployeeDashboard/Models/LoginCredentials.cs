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
    class LoginCredentials : INotifyPropertyChanged
    {
        private string email = "";

        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get => email; set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        private string password = "";

        [JsonProperty(PropertyName = "password")]
        public string Password
        {
            get => password; set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        private int ttl = 1800;

        [JsonProperty(PropertyName = "ttl")]
        public int TTL
        {
            get => ttl; set
            {
                ttl = value;
                OnPropertyChanged("TTL");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
