using IISManager.Implementations;
using System.ComponentModel;

namespace IISManager.ViewModels
{
    public class WorkerProcess : INotifyPropertyChanged
    {
        private int id;
        private string cpuUsage;
        private string memoryUsage;
        private WorkerProcessState state;

        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess workerProcess, ProcessDiagnosticValues processDiagnosticValues)
        {
            id = workerProcess.ProcessId;
            state = (WorkerProcessState)(int)workerProcess.State;
            cpuUsage = processDiagnosticValues.CpuUsage.ToString();
            memoryUsage = processDiagnosticValues.MemoryUsage.ToString();
            ProcessDiagnosticValues = processDiagnosticValues;
        }

        public int Id
        {
            get { return id; }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }

        public WorkerProcessState State
        {
            get { return state; }
            set
            {
                if (this.state != value)
                {
                    this.state = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("State"));
                }
            }
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

        public ProcessDiagnosticValues ProcessDiagnosticValues { get; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
