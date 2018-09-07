﻿using System;
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

        public string URL
        {
            get
            {
                const string localhost = "http://localhost:3000";

                return (string.IsNullOrWhiteSpace(ServerURL) ? localhost : ServerURL) + "/api/";
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

        private string ReadServerURL()
        {
            var serverIP = SummonEmployeeDashboard.Properties.Settings.Default.ServerIP;
            return serverIP;
        }

        private void SaveServerURL(string serverIP)
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
