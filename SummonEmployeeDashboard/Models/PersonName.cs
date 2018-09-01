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
    public class PersonName : INotifyPropertyChanged
    {
        private int id;
        private string firstName = "";
        private string lastName = "";
        private string patronymic = "";

        public string FullName
        {
            get
            {
                return FirstName + " " + (Patronymic != string.Empty ? Patronymic + " " : "") + LastName;
            }
        }

        [JsonProperty(PropertyName = "id")]
        public int Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName
        {
            get => firstName; set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        [JsonProperty(PropertyName = "lastname")]
        public string LastName
        {
            get => lastName; set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        [JsonProperty(PropertyName = "patronymic")]
        public string Patronymic
        {
            get => patronymic; set
            {
                patronymic = value;
                OnPropertyChanged("Patronymic");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
