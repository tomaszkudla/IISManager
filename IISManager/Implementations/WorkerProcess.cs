using IISManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISManager.Implementations
{
    public class WorkerProcess : IWorkerProcess
    {
        private readonly Microsoft.Web.Administration.WorkerProcess workerProcess;
        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess workerProcess)
        {
            this.workerProcess = workerProcess;
        }

        public int Id { get => workerProcess.ProcessId; }
        public WorkerProcessState State { get => (WorkerProcessState)(int)workerProcess.State; }
    }
}
