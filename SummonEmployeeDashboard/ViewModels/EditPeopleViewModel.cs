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
    class EditPeopleViewModel : INotifyPropertyChanged
    {
        public EditPersonViewModel SelectedPersonVM
        {
            get; set;
        }

        public Person SelectedPerson
        {
            get { return SelectedPersonVM.Person; }
            set
            {
                SelectedPersonVM.Person = value;
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

        public EditPeopleViewModel()
        {
            Initialize();
        }

        private async void Initialize()
        {
            AccessToken accessToken = App.GetApp().AccessToken;
            SelectedPersonVM = new EditPersonViewModel();
            People = new ObservableCollection<Person>(await App.GetApp().GetService<PeopleService>()
                .ListPeople(accessToken.Id));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
