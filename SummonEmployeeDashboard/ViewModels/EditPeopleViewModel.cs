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
                selectedPersonVM.GetRole();
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

        private void Initialize()
        {
            Task.Factory.StartNew(() =>
            {
                App app = App.GetApp();
                var accessToken = app.AccessToken;
                SelectedPerson = new EditPersonVM();
                try
                {
                    List<Role> roles = null;
                    var rolesTask = Task.Factory.StartNew(() =>
                    {
                        roles = app.GetService<PeopleService>().ListRoles();
                    });
                    List<Person> people = null;
                    var peopleTask = Task.Factory.StartNew(() =>
                    {
                        people = app.GetService<PeopleService>().ListPeople(accessToken.Id);
                    });
                    Task.WaitAll(rolesTask, peopleTask);
                    app.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        People = new ObservableCollection<EditPersonVM>(
                            people.ConvertAll(p1 => new EditPersonVM() {
                                Person = p1,
                                Roles = new ObservableCollection<Role>(roles)
                            })
                        );
                    }));
                } catch (Exception)
                {
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
