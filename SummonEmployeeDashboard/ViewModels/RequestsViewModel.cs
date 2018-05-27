using Newtonsoft.Json;
using SummonEmployeeDashboard.Models;
using SummonEmployeeDashboard.Rest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.ViewModels
{
    class RequestsViewModel : INotifyPropertyChanged
    {
        private SummonRequest selectedRequest;
        public SummonRequest SelectedRequest
        {
            get { return selectedRequest; }
            set
            {
                selectedRequest = value;
                OnPropertyChanged("SelectedRequest");
            }
        }

        private ObservableCollection<SummonRequest> requests;
        public ObservableCollection<SummonRequest> Requests
        {
            get {
                return requests;
            }
            set {
                requests = value;
                OnPropertyChanged("Requests");
            }
        }

        public RequestsViewModel()
        {
            Initialize();
        }

        private async void Initialize()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
