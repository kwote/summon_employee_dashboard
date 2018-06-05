using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        private IRestClient client = new RestClient("http://192.168.1.25:3000/api/");

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

        public T GetService<T>() where T : IRestService, new() => new T { Client = client };

        public static App GetApp()
        {
            return (App)App.Current;
        }
    }
}
