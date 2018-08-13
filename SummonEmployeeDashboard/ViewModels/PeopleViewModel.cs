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
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class PeopleViewModel : INotifyPropertyChanged
    {
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

        private void Reload()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    App app = App.GetApp();
                    AccessToken accessToken = app.AccessToken;
                    var people = app.GetService<PeopleService>().ListSummonPeople(accessToken.Id);
                    app.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        SelectedPerson = new PersonVM();
                        People = new ObservableCollection<PersonVM>(people.ConvertAll(p => new PersonVM() { Person = p }));
                    }));
                }
                catch (Exception)
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
