using IISManager.Interfaces;
using System;

namespace IISManager.Implementations
{
    public class WorkerProcess : IWorkerProcess, IEquatable<WorkerProcess>
    {
        private readonly Microsoft.Web.Administration.WorkerProcess workerProcess;
        private readonly int id;
        private readonly WorkerProcessState state;
        private readonly double cpuUsage;

        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess workerProcess, double cpuUsage)
        {
            this.workerProcess = workerProcess;
            id = workerProcess.ProcessId;
            state = (WorkerProcessState)(int)workerProcess.State;
            this.cpuUsage = cpuUsage;
        }

        public int Id { get => id; }
        public WorkerProcessState State { get => state; }
        public double CpuUsage { get => cpuUsage; }

        public bool Equals(WorkerProcess other)
        {
            return this.Id == other.Id &&
                this.State == other.State &&
                Math.Abs(this.CpuUsage-other.CpuUsage) < 0.01;
        }
    }
}
