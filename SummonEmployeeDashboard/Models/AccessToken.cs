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
        private int id;
        public int Id
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
        public Person Person { get; set; }
        public bool Expired
        {
            get
            {
                var created = DateTime.Parse(Created);
                var now = DateTime.Now;
                created.AddSeconds((double)TTL);
                return created < now;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
