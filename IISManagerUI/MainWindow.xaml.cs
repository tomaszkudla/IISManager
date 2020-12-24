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
        private ApplicationPoolsManager manager;
        private ObservableCollection<ApplicationPool> applicationPools;
        public MainWindow()
        {
            InitializeComponent();
            manager = new ApplicationPoolsManager();
            applicationPools = new ObservableCollection<ApplicationPool>(manager.GetApplicationPools());
            applicationPoolsControl.ItemsSource = applicationPools;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var applicationPool = menuItem.DataContext as ApplicationPool;
            applicationPool.Start();
            Refresh();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var applicationPool = menuItem.DataContext as ApplicationPool;
            applicationPool.Stop();
            Refresh();
        }

        private void Recycle_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var applicationPool = menuItem.DataContext as ApplicationPool;
            applicationPool.Recycle();
            Refresh();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            applicationPools.Clear();
            var appPools = manager.GetApplicationPools();
            appPools.ForEach(applicationPools.Add);
        }
    }
}
