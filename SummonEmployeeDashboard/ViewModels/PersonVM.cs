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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PersonVM));
        private Person person;
        public Person Person
        {
            get => person;
            set
            {
                bool visibilityChanged = false;
                if ((person == null) != (value == null))
                {
                    visibilityChanged = true;
                }
                bool nameChanged = person?.FullName != value?.FullName;
                person = value;
                OnPropertyChanged("Person");
                if (visibilityChanged)
                {
                    OnPropertyChanged("SelfVisibility");
                }
                if (nameChanged)
                {
                    OnPropertyChanged("FullName");
                }
            }
        }

        private string comment;
        public string Comment
        {
            get => comment; set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

        public Visibility SelfVisibility
        {
            get { return person != null ? Visibility.Visible : Visibility.Hidden; }
        }

        public string FullName
        {
            get
            {
                return person?.Id == App.GetApp().AccessToken?.UserId ? "Я" : person?.FullName;
            }
        }

        public string Online
        {
            get
            {
                var inactive = (person as SummonPerson)?.Inactive;
                if (inactive.HasValue)
                {
                    if (inactive / 1000 <= EventBus.PING_PERIOD)
                    {
                        return "Онлайн";
                    } else
                    {
                        return Utils.ToRelativeDateString(inactive.Value, true);
                    }
                }
                return "Оффлайн";
            }
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
                App app = App.GetApp();
                var accessToken = app.AccessToken;
                var add = new AddSummonRequest()
                {
                    CallerId = accessToken.UserId,
                    TargetId = person.Id,
                    Comment = Comment,
                };
                await app.GetService<SummonRequestService>().AddSummonRequest(add, accessToken.Id);
            }
            catch (Exception e)
            {
                log.Error("Failed to summon person", e);
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
