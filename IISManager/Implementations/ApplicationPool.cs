using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IISManager.Implementations
{
    public class ApplicationPool : IApplicationPool, IEquatable<ApplicationPool>
    {
        private readonly ObservableList<WorkerProcess> workerProcesses = new ObservableList<WorkerProcess>();
        private readonly Microsoft.Web.Administration.ApplicationPool applicationPool;
        private readonly string name;
        private readonly ApplicationPoolState state;

        public ApplicationPool(Microsoft.Web.Administration.ApplicationPool applicationPool)
        {
            this.applicationPool = applicationPool;
            name = applicationPool.Name;
            state = (ApplicationPoolState)(int)applicationPool.State;
            workerProcesses.Value = GetWorkerProcesses(applicationPool);
        }

        public string Name { get => name; }
        public ApplicationPoolState State { get => state; }

        public ObservableList<WorkerProcess> WorkerProcesses => workerProcesses;

        public bool IsSelected { get; set; }

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
                return applicationPool.WorkerProcesses.Select(p => new WorkerProcess(p, 0.0)).ToList();
            }

            return new List<WorkerProcess>();
        }
    }
}
