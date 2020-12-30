using IISManager.Interfaces;
using System;

namespace IISManager.Implementations
{
    public class WorkerProcess : IWorkerProcess, IEquatable<WorkerProcess>
        {
        private readonly Microsoft.Web.Administration.WorkerProcess workerProcess;
        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess workerProcess)
        {
            this.workerProcess = workerProcess;
        }

        public int Id { get => workerProcess.ProcessId; }
        public WorkerProcessState State { get => (WorkerProcessState)(int)workerProcess.State; }

        public bool Equals(WorkerProcess other)
        {
            return this.Id == other.Id &&
                this.State == other.State;
        }
    }
}
