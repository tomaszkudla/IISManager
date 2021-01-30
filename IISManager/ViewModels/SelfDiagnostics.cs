using IISManager.Implementations;
using System.ComponentModel;

namespace IISManager.ViewModels
{
    public class SelfDiagnostics : INotifyPropertyChanged
    {
        private ProcessDiagnostics processDiagnostics;
        private string cpuUsage;
        private string memoryUsage;

        public SelfDiagnostics(ProcessDiagnostics processDiagnostics)
        {
            this.processDiagnostics = processDiagnostics;
            Refresh();
        }

        public string CpuUsage
        {
            get { return cpuUsage; }
            set
            {
                if (this.cpuUsage != value)
                {
                    this.cpuUsage = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CpuUsage"));
                }
            }
        }

        public string MemoryUsage
        {
            get { return memoryUsage; }
            set
            {
                if (this.memoryUsage != value)
                {
                    this.memoryUsage = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("MemoryUsage"));
                }
            }
        }

        public void Refresh()
        {
            var diagnosticValues = processDiagnostics.GetProcessDiagnosticValuesForCurrentProcess();
            CpuUsage = diagnosticValues.CpuUsage.ToString();
            MemoryUsage = diagnosticValues.MemoryUsage.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
