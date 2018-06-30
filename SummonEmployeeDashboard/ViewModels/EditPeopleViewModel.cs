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
        private EditPersonVM selectedPersonVM;

        public EditPersonVM SelectedPerson
        {
            get { return selectedPersonVM; }
            set
            {
                selectedPersonVM = value;
                selectedPersonVM.GetRoleAsync();
                OnPropertyChanged("SelectedPerson");
            }
        }

        private ObservableCollection<EditPersonVM> people;
        public ObservableCollection<EditPersonVM> People {
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
            App app = App.GetApp();
            var accessToken = app.AccessToken;
            SelectedPerson = new EditPersonVM();
            var roles = await app.GetService<PeopleService>().ListRoles();
            var people = await app.GetService<PeopleService>().ListPeople(accessToken.Id);
            People = new ObservableCollection<EditPersonVM>(
                people.ConvertAll(p1 => new EditPersonVM() {
                    Person = p1,
                    Roles = new ObservableCollection<Role>(roles)
                })
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
