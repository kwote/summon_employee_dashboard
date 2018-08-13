using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return id == (obj as Role).id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
