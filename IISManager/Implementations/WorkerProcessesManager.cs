using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.Linq;

namespace IISManager.Implementations
{
    public class WorkerProcessesManager : IWorkerProcessesManager
    {
        private readonly ServerManager serverManager;
        
        public WorkerProcessesManager()
        {
            serverManager = new ServerManager();
        }

        public List<WorkerProcess> GetWorkerProcesses()
        {
            var workerProcesses = serverManager.ApplicationPools.SelectMany(p => p.WorkerProcesses);
            return workerProcesses.Select(p => new WorkerProcess(p)).ToList();
        }
    }
}
