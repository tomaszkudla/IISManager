using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IISManager.Implementations
{
    public class ApplicationPool : IEquatable<ApplicationPool>, INotifyPropertyChanged
    {
        private readonly Microsoft.Web.Administration.ApplicationPool applicationPool;
        private string name;
        private ApplicationPoolState state;
        private bool isSelected;
        private WorkerProcessDiagnostics cpuUsageCounters;

        public ApplicationPool(Microsoft.Web.Administration.ApplicationPool applicationPool)
        {
            this.applicationPool = applicationPool;
            name = applicationPool.Name;
            state = (ApplicationPoolState)(int)applicationPool.State;
            cpuUsageCounters = WorkerProcessDiagnostics.Instance;
            WorkerProcesses.Value = GetWorkerProcesses(applicationPool);
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

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public bool Equals(ApplicationPool other)
        {
            return this.Name == other.Name &&
                this.State == other.State &&
                this.IsSelected == other.IsSelected &&
                Enumerable.SequenceEqual(this.WorkerProcesses.Value, other.WorkerProcesses.Value);
        }

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

        public ApplicationPool Clone()
        {
            return new ApplicationPool(applicationPool)
            {
                IsSelected = this.IsSelected
            };
        }

        private List<WorkerProcess> GetWorkerProcesses(Microsoft.Web.Administration.ApplicationPool applicationPool)
        {
            if (applicationPool.State != ObjectState.Stopped)
            {
                return applicationPool.WorkerProcesses.Select(p => new WorkerProcess(p, cpuUsageCounters.GetWorkerProcessDiagnosticValuesForProcessId(p.ProcessId))).ToList();
            }

            return new List<WorkerProcess>();
        }
    }
}
