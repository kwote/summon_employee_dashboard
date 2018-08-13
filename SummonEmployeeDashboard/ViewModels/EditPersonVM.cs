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
using System.Windows;
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class EditPersonVM : INotifyPropertyChanged
    {
        private Person person;
        public Person Person
        {
            get { return person; }
            set
            {
                person = value;
                OnPropertyChanged("Person");
            }
        }

        private Role role;
        private Role initialRole;
        public Role Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged("Role");
            }
        }

        private ObservableCollection<Role> roles;
        public ObservableCollection<Role> Roles
        {
            get { return roles; }
            set
            {
                roles = value;
                OnPropertyChanged("Roles");
            }
        }

        private ICommand chooseRoleCommand;

        public ICommand ChooseRoleCommand
        {
            get
            {
                if (chooseRoleCommand == null)
                {
                    chooseRoleCommand = new RelayCommand(
                        param => ChooseRole(),
                        param => CanChooseRole()
                    );
                }
                return chooseRoleCommand;
            }
        }

        private void ChooseRole()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    App app = App.GetApp();
                    var accessToken = app.AccessToken;
                    var success = app.GetService<PeopleService>().ChooseRole(person.Id, Role.Name, accessToken.Id);
                    if (success)
                    {
                        app.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            initialRole = Role;
                        }));
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        public void GetRole()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    App app = App.GetApp();
                    var accessToken = app.AccessToken;
                    var r = app.GetService<PeopleService>().GetRole(person.Id, accessToken.Id);
                    app.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Role = initialRole = r;
                    }));
                }
                catch (Exception)
                {
                }
            });
        }

        private bool CanChooseRole()
        {
            return role != initialRole;
        }

        public EditPersonVM()
        {
            Initialize();
        }

        private void Initialize()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
