using IISManager.Implementations;
using IISManager.Utils;
using IISManager.ViewModels;
using IISManagerUI.Converters;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace IISManagerUI
{
    /// <summary>
    /// Interaction logic for AppPoolsControl.xaml
    /// </summary>
    public partial class AppPoolsControl : UserControl
    {
        private readonly ApplicationPoolsManager manager;
        private readonly RefreshingTimer timer;
        private readonly ProcessUtils processUtils;

        public AppPoolsControl(ApplicationPoolsManager manager, RefreshingTimer refreshingTimer, ProcessUtils processUtils)
        {
            InitializeComponent();
            var cpuUsageConverter = Resources["cpuUsageConverter"] as ThresholdConverter;
            cpuUsageConverter.Unit = "%";
            cpuUsageConverter.MediumThreshold = Configuration.CpuUsageMediumThreshold;
            cpuUsageConverter.HighThreshold = Configuration.CpuUsageHighThreshold;
            var memoryUsageConverter = Resources["memoryUsageConverter"] as ThresholdConverter;
            memoryUsageConverter.Unit = "MB";
            var totalMemory = Utils.GetTotalMemory();
            if (totalMemory == null)
            {
                memoryUsageConverter.MediumThreshold = double.MaxValue;
                memoryUsageConverter.HighThreshold = double.MaxValue;
            }
            else
            {
                memoryUsageConverter.MediumThreshold = Configuration.MemoryUsageMediumThreshold * totalMemory.Value / 100.0;
                memoryUsageConverter.HighThreshold = Configuration.MemoryUsageHighThreshold * totalMemory.Value / 100.0;
            }

            this.manager = manager;
            this.timer = refreshingTimer;
            timer.Tick += Timer_Tick;
            this.processUtils = processUtils;
            applicationPoolsControl.DataContext = manager.ApplicationPools;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                manager.Refresh();
            });
        }

        private void CopyId_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Label;
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
        }

        private void KillProcess_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Label;
            var id = int.Parse(button.Tag.ToString());
            processUtils.KillProcess(id);
        }

        private void GoToPath_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Label;
            processUtils.GoToPath(button.Tag.ToString());
        }

        private void SendGetRequest_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Label;
            processUtils.SendGetRequest(button.Tag.ToString());
        }
    }
}
