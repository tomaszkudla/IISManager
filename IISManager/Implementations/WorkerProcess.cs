using IISManager.Interfaces;
using System;
using System.ComponentModel;

namespace IISManager.Implementations
{
    public class WorkerProcess : IEquatable<WorkerProcess>, INotifyPropertyChanged
    {
        private readonly Microsoft.Web.Administration.WorkerProcess workerProcess;
        private int id;
        private WorkerProcessState state;
        private readonly WorkerProcessDiagnosticValues workerProcessDiagnostics;

        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess workerProcess, WorkerProcessDiagnosticValues workerProcessDiagnostics)
        {
            this.workerProcess = workerProcess;
            id = workerProcess.ProcessId;
            state = (WorkerProcessState)(int)workerProcess.State;
            this.workerProcessDiagnostics = workerProcessDiagnostics;
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

        public WorkerProcessDiagnosticValues WorkerProcessDiagnosticValues { get => workerProcessDiagnostics; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public bool Equals(WorkerProcess other)
        {
            return this.Id == other.Id &&
                this.State == other.State &&
                this.workerProcessDiagnostics == other.workerProcessDiagnostics;
        }
    }
}
