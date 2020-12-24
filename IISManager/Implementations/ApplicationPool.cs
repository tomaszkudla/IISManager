using IISManager.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IISManager.Implementations
{
    public class ApplicationPool : IApplicationPool
    {
        private readonly Microsoft.Web.Administration.ApplicationPool applicationPool;
        private readonly List<WorkerProcess> workerProcesses;
        public ApplicationPool(Microsoft.Web.Administration.ApplicationPool applicationPool)
        {
            this.applicationPool = applicationPool;
            workerProcesses = applicationPool.WorkerProcesses.Select(p => new WorkerProcess(p)).ToList();
        }

        public string Name { get => applicationPool.Name; }
        public ApplicationPoolState State { get => (ApplicationPoolState)(int)applicationPool.State; }

        public List<WorkerProcess> WorkerProcesses => workerProcesses;

        public void Recycle()
        {
            applicationPool.Recycle();
        }

        public void Start()
        {
            applicationPool.Start();
        }

        public void Stop()
        {
            applicationPool.Stop();
        }
    }
}
