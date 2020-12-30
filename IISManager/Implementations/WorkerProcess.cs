using IISManager.Interfaces;
using System;

namespace IISManager.Implementations
{
    public class WorkerProcess : IWorkerProcess, IEquatable<WorkerProcess>
    {
        private readonly Microsoft.Web.Administration.WorkerProcess workerProcess;
        private readonly int id;
        private readonly WorkerProcessState state;

        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess workerProcess)
        {
            this.workerProcess = workerProcess;
            id = workerProcess.ProcessId;
            state = (WorkerProcessState)(int)workerProcess.State;
        }

        public int Id { get => id; }
        public WorkerProcessState State { get => state; }

        public bool Equals(WorkerProcess other)
        {
            return this.Id == other.Id &&
                this.State == other.State;
        }
    }
}
