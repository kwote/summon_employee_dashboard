﻿using Newtonsoft.Json;
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
    class EditPersonViewModel : INotifyPropertyChanged
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

        private string role;
        public string Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged("Role");
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
                        async param => await ChooseRoleAsync(),
                        param => CanChooseRole()
                    );
                }
                return chooseRoleCommand;
            }
        }

        private async Task ChooseRoleAsync()
        {
            try
            {
                var accessToken = App.GetApp().AccessToken;
                await App.GetApp().GetService<PeopleService>().ChooseRole(role, accessToken.Id);
            }
            catch (Exception)
            {
            }
        }

        private bool CanChooseRole()
        {
            return true;
        }

        public EditPersonViewModel()
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