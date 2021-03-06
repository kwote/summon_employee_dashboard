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
    class SummonRequestVM : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SummonRequestVM));
        public PersonVM Person { get; private set; }
        public Action CloseAction { get; set; }

        private SummonRequest request;

        public SummonRequest Request
        {
            get
            {
                return request;
            }
            set
            {
                request = value;
                Person = new PersonVM() { Person = incoming ? request.Caller : request.Target };
                OnPropertyChanged("Request");
                OnPropertyChanged("SelfVisibility");
            }
        }

        public string State
        {
            get
            {
                if (!request.Enabled) return "Отменён";
                switch (request.State)
                {
                    case RequestState.Pending:
                        return "Ожидает";
                    case RequestState.Accepted:
                        return "Принят";
                    case RequestState.Rejected:
                        return "Отклонён";
                    default:
                        return "";
                }
            }
        }

        private bool IsPending()
        {
            return request != null && request.Enabled && request.State == RequestState.Pending;
        }

        public Visibility SelfVisibility
        {
            get { return request != null ? Visibility.Visible : Visibility.Hidden; }
        }

        public Visibility CancelVisible
        {
            get { return CanCancel() ? Visibility.Visible : Visibility.Hidden; }
        }

        public Visibility AcceptVisible
        {
            get { return CanAccept() ? Visibility.Visible : Visibility.Hidden; }
        }

        public Visibility RejectVisible
        {
            get { return CanReject() ? Visibility.Visible : Visibility.Hidden; }
        }

        private readonly bool incoming;
        public bool Incoming { get; }

        private ICommand acceptCommand;

        public ICommand AcceptCommand
        {
            get
            {
                if (acceptCommand == null)
                {
                    acceptCommand = new RelayCommand(
                        param => Accept(),
                        param => CanAccept()
                    );
                }
                return acceptCommand;
            }
        }

        private async void Accept()
        {
            try
            {
                App app = App.GetApp();
                var accessToken = app.AccessToken;
                await app.GetService<SummonRequestService>()
                    .Accept(Request.Id, accessToken.Id);
                CloseAction?.Invoke();
            }
            catch (Exception e)
            {
                log.Error("Failed to accept request", e);
            }
        }

        private bool CanAccept()
        {
            return IsPending() && incoming;
        }

        private ICommand rejectCommand;

        public ICommand RejectCommand
        {
            get
            {
                if (rejectCommand == null)
                {
                    rejectCommand = new RelayCommand(
                        param => Reject(),
                        param => CanReject()
                    );
                }
                return rejectCommand;
            }
        }

        private async void Reject()
        {
            try
            {
                App app = App.GetApp();
                var accessToken = app.AccessToken;
                await app.GetService<SummonRequestService>()
                    .Reject(Request.Id, accessToken.Id);
                CloseAction?.Invoke();
            }
            catch (Exception e)
            {
                log.Error("Failed to reject request", e);
            }
        }

        private bool CanReject()
        {
            return IsPending() && incoming;
        }

        private ICommand cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(
                        param => Cancel(),
                        param => CanCancel()
                    );
                }
                return cancelCommand;
            }
        }

        private async void Cancel()
        {
            try
            {
                App app = App.GetApp();
                var accessToken = app.AccessToken;
                await app.GetService<SummonRequestService>()
                    .Cancel(Request.Id, accessToken.Id);
            }
            catch (Exception e)
            {
                log.Error("Failed to cancel request", e);
            }
        }

        private bool CanCancel()
        {
            return request != null && request.Enabled && !incoming;
        }

        public SummonRequestVM(bool incoming)
        {
            this.incoming = incoming;
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
