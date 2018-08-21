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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EditPersonVM));
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
                App app = App.GetApp();
                var accessToken = app.AccessToken;
                var success = await app.GetService<PeopleService>().ChooseRole(person.Id, Role.Name, accessToken.Id);
                if (success)
                {
                    initialRole = Role;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public async void GetRole()
        {
            try
            {
                App app = App.GetApp();
                var accessToken = app.AccessToken;
                var r = await app.GetService<PeopleService>().GetRole(person.Id, accessToken.Id);
                Role = initialRole = r;
            }
            catch (Exception e)
            {
                log.Error("Failed to get role", e);
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
