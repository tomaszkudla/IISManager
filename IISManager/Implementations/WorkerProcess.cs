using IISManager.Interfaces;
using System;
using System.ComponentModel;

namespace IISManager.Implementations
{
    public class WorkerProcess : IEquatable<WorkerProcess>, INotifyPropertyChanged
    {
        private int id;
        private string cpuUsage;
        private string memoryUsage;
        private WorkerProcessState state;

        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess workerProcess, WorkerProcessDiagnosticValues workerProcessDiagnostics)
        {
            id = workerProcess.ProcessId;
            state = (WorkerProcessState)(int)workerProcess.State;
            cpuUsage = workerProcessDiagnostics.CpuUsage.ToString();
            memoryUsage = workerProcessDiagnostics.MemoryUsage.ToString();
            WorkerProcessDiagnostics = workerProcessDiagnostics;
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

        public WorkerProcessDiagnosticValues WorkerProcessDiagnostics { get; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public bool Equals(WorkerProcess other)
        {
            return this.Id == other.Id &&
                this.State == other.State &&
                this.CpuUsage == other.CpuUsage &&
                this.MemoryUsage == other.MemoryUsage;
        }
    }
}
