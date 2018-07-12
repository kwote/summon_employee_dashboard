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
    class PersonVM : INotifyPropertyChanged
    {
        private Person person;
        public Person Person
        {
            get { return person; }
            set
            {
                person = value;
                OnPropertyChanged("Person");
                OnPropertyChanged("SelfVisibility");
            }
        }

        public Visibility SelfVisibility
        {
            get { return person != null ? Visibility.Visible : Visibility.Hidden; }
        }

        private ICommand summonCommand;

        public ICommand SummonCommand
        {
            get
            {
                if (summonCommand == null)
                {
                    summonCommand = new RelayCommand(
                        param => Summon(),
                        param => CanSummon()
                    );
                }
                return summonCommand;
            }
        }

        public PersonVM()
        {
            Initialize();
        }

        private bool CanSummon()
        {
            var accessToken = App.GetApp().AccessToken;
            return accessToken?.UserId != person?.Id;
        }

        private async void Summon()
        {
            try
            {
                var accessToken = App.GetApp().AccessToken;
                var add = new AddSummonRequest()
                {
                    CallerId = accessToken.UserId,
                    TargetId = person.Id
                };
                await App.GetApp().GetService<SummonRequestService>().AddSummonRequest(add, accessToken.Id);
            }
            catch (Exception)
            {
            }
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