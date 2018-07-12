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
                        async param => await ChooseRole(),
                        param => CanChooseRole()
                    );
                }
                return chooseRoleCommand;
            }
        }

        private async Task ChooseRole()
        {
            try
            {
                var accessToken = App.GetApp().AccessToken;
                var success = await App.GetApp().GetService<PeopleService>().ChooseRole(person.Id, Role.Name, accessToken.Id);
                if (success)
                {
                    initialRole = Role;
                }
            }
            catch (Exception)
            {
            }
        }

        public async void GetRole()
        {
            try
            {
                var accessToken = App.GetApp().AccessToken;
                var r = await App.GetApp().GetService<PeopleService>().GetRole(person.Id, accessToken.Id);
                Role = initialRole = r;
            }
            catch (Exception)
            {
            }
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
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
