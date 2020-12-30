﻿using IISManager.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for AppPoolsControl.xaml
    /// </summary>
    public partial class AppPoolsControl : UserControl
    {
        private readonly ApplicationPoolsManager manager;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public AppPoolsControl()
        {
            InitializeComponent();
            manager = ApplicationPoolsManager.Instance;
            applicationPoolsControl.DataContext = manager.ApplicationPools;
            SetupTimer();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                manager.Refresh();
            });
        }

        private void SetupTimer()
        {
            timer.Tick += DispatcherTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                var menuItem = sender as MenuItem;
                var applicationPool = menuItem.DataContext as ApplicationPool;
                applicationPool.Start();
                manager.Refresh();
            });
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                var menuItem = sender as MenuItem;
                var applicationPool = menuItem.DataContext as ApplicationPool;
                applicationPool.Stop();
                manager.Refresh();
            });
        }
        private void Recycle_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                var menuItem = sender as MenuItem;
                var applicationPool = menuItem.DataContext as ApplicationPool;
                applicationPool.Recycle();
                manager.Refresh();
            });
        }

        private void CopyId_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                var button = sender as Label;
                var id = button.Tag.ToString();
                Clipboard.SetText(id);
            });
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                var checkBox = sender as CheckBox;
                var appPoolName = checkBox.Tag.ToString();
                manager.Select(appPoolName);
            });
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                var checkBox = sender as CheckBox;
                var appPoolName = checkBox.Tag.ToString();
                manager.Unselect(appPoolName);
            });
        }
    }
}