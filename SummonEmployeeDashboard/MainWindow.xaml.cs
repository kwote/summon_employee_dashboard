using RestSharp;
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
        private MainViewModel viewModel = new MainViewModel(new List<Person>
            {
                new Person{ FirstName = "Ernest", LastName = "Asanov", Patronymic = "Edemovich" },
            });
        public MainWindow()
        {
            InitializeComponent();

            var client = new RestClient("http://192.168.1.12:3000/api/");
            var request = new RestRequest("people");
            request.AddQueryParameter("departmentId", "1");
            var asyncHandle = client.ExecuteAsync<List<Person>>(request, response =>
            {
                viewModel.People = new ObservableCollection<Person>(response.Data);
            });

            peopleTab.DataContext = viewModel;
        }
    }
}
