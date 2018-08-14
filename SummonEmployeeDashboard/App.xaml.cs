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
            const string appName = "SummonEmployeeDashboard";

            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                //app is already running! Exiting the application  
                Current.Shutdown();
            }

            base.OnStartup(e);
        }

        private string serverIP = null;
        public string ServerIP
        {
            get
            {
                if (serverIP == null)
                {
                    serverIP = ReadServerIP();
                }
                return serverIP;
            }
            set
            {
                serverIP = value;
                client = new RestClient(URL);
                SaveServerIP(serverIP);
            }
        }

        public string URL
        {
            get
            {
                const string localhost = "http://localhost:3000";

                return (string.IsNullOrWhiteSpace(ServerIP) ? localhost : ServerIP) + "/api/";
            }
        }
        private IRestClient client = null;

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

        private string ReadServerIP()
        {
            var serverIP = SummonEmployeeDashboard.Properties.Settings.Default.ServerIP;
            return serverIP;
        }

        private void SaveServerIP(string serverIP)
        {
            SummonEmployeeDashboard.Properties.Settings.Default.ServerIP = serverIP;
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
