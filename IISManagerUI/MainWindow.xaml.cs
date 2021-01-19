using System.Windows;

namespace IISManagerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.LocationChanged += MainWindow_LocationChanged;
            InitializeComponent();
        }

        private void MainWindow_LocationChanged(object sender, System.EventArgs e)
        {
            RefreshingTimer.Instance.PauseForNextTick();
        }

    }
}
