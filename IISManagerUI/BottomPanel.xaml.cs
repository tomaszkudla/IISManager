using IISManager.Utils;
using IISManager.ViewModels;
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

namespace IISManagerUI
{
    /// <summary>
    /// Interaction logic for BottomPanel.xaml
    /// </summary>
    public partial class BottomPanel : UserControl
    {
        private readonly RefreshingTimer timer;
        private readonly SelfDiagnostics selfDiagnostics;
        private readonly UserMessage userMessage;

        public BottomPanel(SelfDiagnostics selfDiagnostics, RefreshingTimer refreshingTimer, UserMessage userMessage)
        {
            InitializeComponent();
            this.selfDiagnostics = selfDiagnostics;
            selfDiagnosticsPanel.DataContext = selfDiagnostics;
            timer = refreshingTimer;
            timer.Tick += Timer_Tick;
            this.userMessage = userMessage;
            userMessagePanel.DataContext = userMessage;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Task.Run(() => selfDiagnostics.Refresh());
        }

        private void OpenLogsDir_Click(object sender, MouseButtonEventArgs e)
        {
            Utils.SafeExecute(() =>
            {
                ProcessUtils.GoToPath(Configuration.LogsDirPath);
            });
        }
    }
}
