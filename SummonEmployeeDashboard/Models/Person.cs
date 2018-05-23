using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard
{
    class Person : INotifyPropertyChanged
    {
        private int id;
        public int Id { get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string FullName
        {
            get
            {
                return FirstName + " " + (Patronymic != null ? Patronymic + " " : "") + LastName;
            }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Post { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? DepartmentId { get; set; }
        public string LastActiveTime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
