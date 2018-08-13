using SummonEmployeeDashboard.Models;
using SummonEmployeeDashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SummonEmployeeDashboard
{
    /// <summary>
    /// Логика взаимодействия для SummonRequestWindow.xaml
    /// </summary>
    public partial class SummonRequestWindow : Window
    {
        private SummonRequestVM viewModel;

        public SummonRequestWindow(SummonRequest request)
        {
            InitializeComponent();
            viewModel = new SummonRequestVM(true) { Request = request, CloseAction = () => { Close(); } };
            DataContext = viewModel;
            SystemSounds.Asterisk.Play();
        }
    }
}
