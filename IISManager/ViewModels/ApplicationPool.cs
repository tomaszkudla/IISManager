using IISManager.Implementations;
using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IISManager.ViewModels
{
    public class ApplicationPool : INotifyPropertyChanged
    {
        private readonly Microsoft.Web.Administration.ApplicationPool applicationPool;
        private string name;
        private ApplicationPoolState state;
        private bool isSelected;
        private readonly ProcessDiagnostics processDiagnostics;

        public ApplicationPool(Microsoft.Web.Administration.ApplicationPool applicationPool, List<Application> applications, ProcessDiagnostics processDiagnostics)
        {
            this.applicationPool = applicationPool;
            name = applicationPool.Name;
            state = (ApplicationPoolState)(int)applicationPool.State;
            this.processDiagnostics = processDiagnostics;
            WorkerProcesses.Value = GetWorkerProcesses(applicationPool);
            Applications = new ApplicationsList(applications);
        }

        public string Name 
        { 
            get { return name; } 
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        public ApplicationPoolState State
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

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (this.IsSelected != value)
                {
                    this.isSelected = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
        }

        public WorkerProcessesList WorkerProcesses { get; set; } = new WorkerProcessesList();

        public ApplicationsList Applications { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void Recycle()
        {
            if (applicationPool.State != ObjectState.Stopped)
            {
                applicationPool.Recycle();
            }
        }

        public void Start()
        {
            if (applicationPool.State != ObjectState.Started)
            {
                applicationPool.Start();
            }
        }

        public void Stop()
        {
            if (applicationPool.State != ObjectState.Stopped)
            {
                applicationPool.Stop();
            }
        }

        private List<WorkerProcess> GetWorkerProcesses(Microsoft.Web.Administration.ApplicationPool applicationPool)
        {
            if (applicationPool.State != ObjectState.Stopped)
            {
                return applicationPool.WorkerProcesses.Select(p => new WorkerProcess(p, processDiagnostics.GetProcessDiagnosticValuesByProcessId(p.ProcessId))).ToList();
            }

            return new List<WorkerProcess>();
        }
    }
}
