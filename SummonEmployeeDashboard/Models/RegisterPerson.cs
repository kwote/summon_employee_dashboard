using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard
{
    class RegisterPerson : INotifyPropertyChanged
    {
        public string FullName
        {
            get
            {
                return FirstName + " " + (Patronymic != null ? Patronymic + " " : "") + LastName;
            }
        }

        private string _firstName = "";
        private string _lastName = "";
        private string _patronymic = "";
        private string _email = "";
        private string _phone = "";
        private string _post = "";
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName
        {
            get => _firstName; set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        [JsonProperty(PropertyName = "lastname")]
        public string LastName
        {
            get => _lastName; set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        [JsonProperty(PropertyName = "patronymic")]
        public string Patronymic
        {
            get => _patronymic; set
            {
                _patronymic = value;
                OnPropertyChanged("Patronymic");
            }
        }
        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get => _email; set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        [JsonProperty(PropertyName = "phone")]
        public string Phone
        {
            get => _phone; set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }
        [JsonProperty(PropertyName = "post")]
        public string Post
        {
            get => _post; set
            {
                _post = value;
                OnPropertyChanged("Post");
            }
        }
        private string _password = "";
        [JsonProperty(PropertyName = "password")]
        public string Password
        {
            get => _password; set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
