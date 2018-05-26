using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Models
{
    class AccessToken : INotifyPropertyChanged
    {
        private string id;
        public string Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public int? TTL { get; set; }
        public string Created { get; set; }
        public int UserId { get; set; }
        public Person User { get; set; }
        public bool Expired()
        {
            var created = DateTime.Parse(Created);
            var now = DateTime.Now;
            created = created.AddSeconds((double)TTL);
            return created < now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
