using IISManager.Implementations;
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

namespace IISManagerUI
{
    /// <summary>
    /// Interaction logic for TopPanel.xaml
    /// </summary>
    public partial class TopPanel : UserControl
    {
        private readonly ApplicationPoolsManager manager;
        private readonly IISServerManager iisServerManager;

        public TopPanel()
        {
            InitializeComponent();
            manager = ApplicationPoolsManager.Instance;
            iisServerManager = IISServerManager.Instance;
            //selectAllCheckBox.SetBinding(CheckBox.IsCheckedProperty, new Binding("test")
            //{
            //    Source = manager.AllSelected
            //});
            selectAllCheckBox.DataContext = manager.AllSelected;
            //var xBinding = new Binding();
            //a real instance of the object where the source property is defined
            //it have to be the same instance which is defined in DataModel.DetailList
            //xBinding.Source = manager.AllSelected;
            //xBinding.Path = new PropertyPath("Value");
            //xBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //xBinding.Mode = BindingMode.OneWay;
            //Use this instead the .SetBinding( , ) where the checkbox is the object to binded to
            //BindingOperations.SetBinding(selectAllCheckBox, CheckBox.ContentProperty, xBinding);
            //selectAllCheckBox.SetBinding(CheckBox.ContentProperty, xBinding);
        }

        private void StartSelected_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                manager.StartSelected();
            });
        }

        private void StopSelected_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                manager.StopSelected();
            });
        }

        private void RecycleSelected_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                manager.RecycleSelected();
            });
        }

        private void StartIIS_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                iisServerManager.Start();
            });
        }

        private void StopIIS_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                iisServerManager.Stop();
            });
        }

        private void ResetIIS_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                iisServerManager.Reset();
            });
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Utils.SafeExecute(() =>
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
            });
        }
    }
}
