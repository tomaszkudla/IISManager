using System.Windows;

namespace IISManagerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(AppPoolsControl appPoolsControl, TopPanel topPanel)
        {
            InitializeComponent();
            topPanelBorder.Child = topPanel;
            appPoolsScrollViewer.Content = appPoolsControl;
        }
    }
}
