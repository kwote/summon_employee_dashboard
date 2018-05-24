using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RestSharp;
using SummonEmployeeDashboard.Rest;

namespace SummonEmployeeDashboard
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IRestClient client = new RestClient("http://192.168.1.12:3000/api/");
        public IRestClient RestService {
            get {
                return client;
            }
        }

        public T GetService<T>() where T : IRestService, new() => new T { Client = client };
    }
}
