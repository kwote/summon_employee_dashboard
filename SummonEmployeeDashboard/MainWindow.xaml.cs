using RestSharp;
using SummonEmployeeDashboard.Models;
using SummonEmployeeDashboard.Rest;
using SummonEmployeeDashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SummonEmployeeDashboard
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            var accessToken = ReadAccessToken();
            if (accessToken == null)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }

            peopleTab.DataContext = viewModel;
        }

        private AccessToken ReadAccessToken()
        {
            var tokenStr = Properties.Settings.Default.AccessToken;
            if (tokenStr == string.Empty)
            {
                return null;
            }
            var token = SimpleJson.SimpleJson.DeserializeObject<AccessToken>(tokenStr);
            if (token.Expired)
            {
                return null;
            }
            return token;
        }
    }
}
