using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IISManager.Implementations
{
    public class ApplicationPool : IApplicationPool
    {
        private readonly ObservableCollection<WorkerProcess> workerProcesses = new ObservableCollection<WorkerProcess>();
        private readonly Microsoft.Web.Administration.ApplicationPool applicationPool;

        public ApplicationPool(Microsoft.Web.Administration.ApplicationPool applicationPool)
        {
            this.applicationPool = applicationPool;
            var workerProcessesList = GetWorkerProcesses();
            workerProcessesList.ForEach(workerProcesses.Add);
        }

        public string Name { get => applicationPool.Name; }
        public ApplicationPoolState State { get => (ApplicationPoolState)(int)applicationPool.State; }

        public ObservableCollection<WorkerProcess> WorkerProcesses => workerProcesses;

        public bool IsSelected { get; set; }

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

        private List<WorkerProcess> GetWorkerProcesses()
        {
            if (applicationPool.State == ObjectState.Started)
            {
                return applicationPool.WorkerProcesses.Select(p => new WorkerProcess(p)).ToList();
            }

            return new List<WorkerProcess>();
        }
    }
}
