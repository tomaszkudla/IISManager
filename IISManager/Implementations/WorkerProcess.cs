using IISManager.Interfaces;
using System;

namespace IISManager.Implementations
{
    public class WorkerProcess : IEquatable<WorkerProcess>
    {
        private readonly Microsoft.Web.Administration.WorkerProcess workerProcess;
        private readonly int id;
        private readonly WorkerProcessState state;
        private readonly WorkerProcessDiagnosticValues workerProcessDiagnostics;

        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess workerProcess, WorkerProcessDiagnosticValues workerProcessDiagnostics)
        {
            this.workerProcess = workerProcess;
            id = workerProcess.ProcessId;
            state = (WorkerProcessState)(int)workerProcess.State;
            this.workerProcessDiagnostics = workerProcessDiagnostics;
        }

        public int Id { get => id; }
        public WorkerProcessState State { get => state; }
        public WorkerProcessDiagnosticValues WorkerProcessDiagnosticValues { get => workerProcessDiagnostics; }

        public bool Equals(WorkerProcess other)
        {
            return this.Id == other.Id &&
                this.State == other.State &&
                this.workerProcessDiagnostics == other.workerProcessDiagnostics;
        }
    }
}
