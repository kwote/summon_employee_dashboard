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
    class PeopleStatsViewModel : INotifyPropertyChanged
    {
        private PersonWithStatsVM selectedPersonVM;

        public PersonWithStatsVM SelectedPerson
        {
            get { return selectedPersonVM; }
            set
            {
                selectedPersonVM = value;
                if (selectedPersonVM.Stats == null)
                {
                    selectedPersonVM.Stats = new StatisticsViewModel(selectedPersonVM.Person.Person.Id);
                }
                OnPropertyChanged("SelectedPerson");
            }
        }

        private ObservableCollection<PersonWithStatsVM> people;
        public ObservableCollection<PersonWithStatsVM> People {
            get {
                return people;
            }
            set {
                people = value;
                OnPropertyChanged("People");
            }
        }

        public PeopleStatsViewModel()
        {
            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                App app = App.GetApp();
                var accessToken = app.AccessToken;
                var people = await app.GetService<PeopleService>().ListPeople(accessToken.Id);
                People = new ObservableCollection<PersonWithStatsVM>(
                    people.ConvertAll(p => new PersonWithStatsVM() { Person = new PersonVM() { Person = p } })
                );
            } catch (Exception)
            {
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
