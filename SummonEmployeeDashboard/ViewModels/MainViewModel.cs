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
    class MainViewModel : INotifyPropertyChanged
    {
        private Person selectedPerson;
        public Person SelectedPerson
        {
            get { return selectedPerson; }
            set
            {
                selectedPerson = value;
                OnPropertyChanged("SelectedPerson");
            }
        }

        private ObservableCollection<Person> people;
        public ObservableCollection<Person> People {
            get {
                return people;
            }
            set {
                people = value;
                OnPropertyChanged("People");
            }
        }

        public Action CloseAction { get; set; }

        public MainViewModel(Action closeAction)
        {
            CloseAction = closeAction;
            Initialize();
        }

        private async void Initialize()
        {
            if (App.GetApp().AccessToken == null)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                CloseAction();
                return;
            }
            var departmentId = App.GetApp().AccessToken.User.DepartmentId;
            People = new ObservableCollection<Person>(await App.GetApp().GetService<PeopleService>().ListPeople(departmentId));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
