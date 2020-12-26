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
using System.Windows.Threading;

namespace IISManagerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationPoolsManager manager;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            manager = new ApplicationPoolsManager();
            applicationPoolsControl.ItemsSource = manager.ApplicationPools;
            SetupTimer();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            manager.Refresh();
        }

        private void SetupTimer()
        {
            timer.Tick += DispatcherTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
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

        private void CopyId_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var id = button.Tag.ToString();
            Clipboard.SetText(id);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var appPoolName = checkBox.Tag.ToString();
            manager.Select(appPoolName);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var appPoolName = checkBox.Tag.ToString();
            manager.Unselect(appPoolName);
            selectAllCheckBox.IsChecked = false;
        }

        private void AllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            manager.SelectAll();
        }

        private void AllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            manager.UnselectAll();
        }

        private void StartSelected_Click(object sender, RoutedEventArgs e)
        {
            manager.StartSelected();
        }

        private void StopSelected_Click(object sender, RoutedEventArgs e)
        {
            manager.StopSelected();
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                manager.SelectAll();
            }
            else
            {
                manager.UnselectAll();
            }
        }
    }
}
