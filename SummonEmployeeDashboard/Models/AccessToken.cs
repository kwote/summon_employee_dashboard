using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Models
{
    public class AccessToken : INotifyPropertyChanged
    {
        private string _id;
        private int? _ttl;
        private string _created;
        private int _userId;
        private Person _user;

        public string Id
        {
            get => _id; set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public int? TTL
        {
            get => _ttl; set
            {
                _ttl = value;
                OnPropertyChanged("TTL");
            }
        }
        public string Created
        {
            get => _created; set
            {
                _created = value;
                OnPropertyChanged("Created");
            }
        }
        public int UserId
        {
            get => _userId; set
            {
                _userId = value;
                OnPropertyChanged("UserId");
            }
        }
        public Person User
        {
            get => _user; set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }
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
