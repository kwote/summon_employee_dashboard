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
    class Role : INotifyPropertyChanged
    {
        private int id;
        private string name = "";
        
        [JsonProperty(PropertyName = "id")]
        public int Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get => name; set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
