using IISManager.Implementations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IISManagerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationPoolsManager manager;
        public MainWindow()
        {
            InitializeComponent();
            manager = new ApplicationPoolsManager();
            applicationPoolsControl.ItemsSource = manager.ApplicationPools;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var applicationPool = menuItem.DataContext as ApplicationPool;
            applicationPool.Start();
            manager.Refresh();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var applicationPool = menuItem.DataContext as ApplicationPool;
            applicationPool.Stop();
            manager.Refresh();
        }

        private void Recycle_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var applicationPool = menuItem.DataContext as ApplicationPool;
            applicationPool.Recycle();
            manager.Refresh();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            manager.Refresh();
        }
    }
}
