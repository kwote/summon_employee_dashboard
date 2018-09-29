using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using RestSharp;
using SummonEmployeeDashboard.Models;
using SummonEmployeeDashboard.Rest;

namespace SummonEmployeeDashboard
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "SummonEmployee";

            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                //app is already running! Exiting the application  
                Current.Shutdown();
            }

            base.OnStartup(e);
        }

        private string serverURL = null;
        public string ServerURL
        {
            get
            {
                if (serverURL == null)
                {
                    serverURL = ReadServerURL();
                }
                return serverURL;
            }
            set
            {
                serverURL = value.Trim();
                client = new RestClient(URL);
                SaveServerURL(serverURL);
            }
        }

        private string ReadServerURL()
        {
            var serverIP = SummonEmployeeDashboard.Properties.Settings.Default.ServerURL;
            return serverIP;
        }

        private void SaveServerURL(string serverURL)
        {
            SummonEmployeeDashboard.Properties.Settings.Default.ServerURL = serverURL;
            SummonEmployeeDashboard.Properties.Settings.Default.Save();
        }

        public string URL
        {
            get
            {
                const string localhost = "http://localhost:3000";

                return (string.IsNullOrWhiteSpace(ServerURL) ? localhost : ServerURL) + "/api/";
            }
        }
        private IRestClient client = null;
        public EventBus EventBus { get; } = new EventBus();

        private AccessToken accessToken = null;
        internal AccessToken AccessToken {
            get
            {
                if (accessToken == null)
                {
                    accessToken = ReadAccessToken();
                }
                return accessToken;
            }
            set
            {
                accessToken = value;
                SaveAccessToken(accessToken);
            }
        }

        private AccessToken ReadAccessToken()
        {
            var tokenStr = SummonEmployeeDashboard.Properties.Settings.Default.AccessToken;
            if (tokenStr == string.Empty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<AccessToken>(tokenStr);
        }

        private void SaveAccessToken(AccessToken accessToken)
        {
            var tokenStr = JsonConvert.SerializeObject(accessToken);
            SummonEmployeeDashboard.Properties.Settings.Default.AccessToken = tokenStr;
            SummonEmployeeDashboard.Properties.Settings.Default.Save();
        }

        private string login = null;
        internal string Login
        {
            get
            {
                if (login == null)
                {
                    login = ReadLogin();
                }
                return login;
            }
            set
            {
                login = value;
                SaveLogin(login);
            }
        }

        private string ReadLogin()
        {
            return SummonEmployeeDashboard.Properties.Settings.Default.Login;
        }

        private void SaveLogin(string login)
        {
            SummonEmployeeDashboard.Properties.Settings.Default.Login = login;
            SummonEmployeeDashboard.Properties.Settings.Default.Save();
        }

        public T GetService<T>() where T : IRestService, new()
        {
            if (client == null)
            {
                client = new RestClient(URL);
            }
            return new T { Client = client };
        }

        public static App GetApp()
        {
            return (App)Current;
        }
    }
}
