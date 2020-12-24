using IISManager.Implementations;
using System.Collections.Generic;

namespace IISManager.Interfaces
{
    public interface IWorkerProcessesManager
    {
        List<WorkerProcess> GetWorkerProcesses();
    }
}
