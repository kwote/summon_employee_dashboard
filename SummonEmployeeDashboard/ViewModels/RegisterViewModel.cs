using SummonEmployeeDashboard.Models;
using SummonEmployeeDashboard.Rest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SummonEmployeeDashboard.ViewModels
{
    class RegisterViewModel : INotifyPropertyChanged
    {
        private RegisterPerson registerPerson;
        public RegisterPerson RegisterPerson
        {
            get { return registerPerson; }
            set
            {
                registerPerson = value;
                OnPropertyChanged("RegisterPerson");
            }
        }
        public string Password { get => RegisterPerson.Password; set => RegisterPerson.Password = value; }
        public string PasswordConfirm { get; set; }

        private string error;
        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                OnPropertyChanged("Error");
            }
        }

        public Action CloseAction { get; set; }

        public RegisterViewModel(Action action)
        {
            CloseAction = action;
            Initialize();
        }

        private void Initialize()
        {
            registerPerson = new RegisterPerson();
        }

        private ICommand registerCommand;

        public ICommand RegisterCommand
        {
            get
            {
                if (registerCommand == null)
                {
                    registerCommand = new RelayCommand(
                        param => RegisterAsync(),
                        param => CanRegister()
                    );
                }
                return registerCommand;
            }
        }

        private bool CanRegister()
        {
            if (!registerPerson.Email.Contains('@'))
            {
                return false;
            }
            if (registerPerson.Password.Length < 5)
            {
                return false;
            }
            if (registerPerson.FirstName == string.Empty)
            {
                return false;
            }
            if (registerPerson.LastName == string.Empty)
            {
                return false;
            }
            if (!IsPhoneValid(registerPerson.Phone))
            {
                return false;
            }
            if (registerPerson.Password != PasswordConfirm)
            {
                return false;
            }
            return true;
        }

        Regex regex = new Regex("^+([0-9-]?){9,11}[0-9]$");
        private bool IsPhoneValid(string phone)
        {
            if (phone != string.Empty)
            {
                return regex.IsMatch(phone);
            }
            return true;
        }

        private void RegisterAsync()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    App app = App.GetApp();
                    var person = app.GetService<PeopleService>().Register(registerPerson);
                    if (person != null)
                    {
                        app.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var loginWindow = new LoginWindow();
                            loginWindow.Show();
                            CloseAction();
                        }));
                    }
                }
                catch (Exception e)
                {
                    Error = e.Message;
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
