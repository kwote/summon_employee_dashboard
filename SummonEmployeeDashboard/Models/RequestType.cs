using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SummonEmployeeDashboard.Models
{
    class RequestType : INotifyPropertyChanged
    {
        public enum RequestTypeEnum
        {
            Incoming = 1,
            Outgoing = 2,
        }
        public static RequestType[] Types()
        {
            return new RequestType[] {
                Incoming(),
                Outgoing(),
            };
        }
        private RequestTypeEnum id;
        private string name = "";

        public static RequestType Incoming()
        {
            return new RequestType() { Id = RequestTypeEnum.Incoming, Name = "Входящие" };
        }

        public static RequestType Outgoing()
        {
            return new RequestType() { Id = RequestTypeEnum.Outgoing, Name = "Исходящие" };
        }

        public RequestTypeEnum Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

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

            return id == (obj as RequestType).id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
