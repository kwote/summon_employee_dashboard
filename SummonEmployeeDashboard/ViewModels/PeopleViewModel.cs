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
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class PeopleViewModel : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PeopleViewModel));
        private PersonVM selectedPersonVM;
        public PersonVM SelectedPerson
        {
            get { return selectedPersonVM; }
            set
            {
                selectedPersonVM = value;
                OnPropertyChanged("SelectedPerson");
            }
        }

        private ObservableCollection<PersonVM> people;
        public ObservableCollection<PersonVM> People {
            get {
                return people;
            }
            set {
                people = value;
                OnPropertyChanged("People");
            }
        }

        private ICommand reloadCommand;

        public ICommand ReloadCommand
        {
            get
            {
                if (reloadCommand == null)
                {
                    reloadCommand = new RelayCommand(
                        param => Reload(),
                        param => CanReload()
                    );
                }
                return reloadCommand;
            }
        }

        private bool CanReload()
        {
            return true;
        }

        public PeopleViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            Reload();
        }

        private async void Reload()
        {
            try
            {
                App app = App.GetApp();
                AccessToken accessToken = app.AccessToken;
                var people = await app.GetService<PeopleService>().ListSummonPeople(accessToken.Id);
                SelectedPerson = new PersonVM();
                People = new ObservableCollection<PersonVM>(people.ConvertAll(p => new PersonVM() { Person = p }));
            }
            catch (Exception e)
            {
                log.Error("Failed to list summon people", e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
