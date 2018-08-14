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

        private readonly SynchronizationContext syncContext;

        public SummonRequestWindow(SummonRequest request)
        {
            InitializeComponent();
            syncContext = SynchronizationContext.Current;
            viewModel = new SummonRequestVM(true) { Request = request, CloseAction = () => { Close(); } };
            DataContext = viewModel;
            EventBus.Instance.Register(this);
            SystemSounds.Asterisk.Play();
        }

        public void OnEvent(SummonRequestUpdate update)
        {
            switch (update.UpdateType)
            {
                case UpdateType.Create:
                    syncContext.Post(o =>
                    {
                    }, null);
                    break;
                case UpdateType.Cancel:
                    syncContext.Post(o =>
                    {
                        Close();
                    }, null);
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            EventBus.Instance.Unregister(this);
        }
    }
}
