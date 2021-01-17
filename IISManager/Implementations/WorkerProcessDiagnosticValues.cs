using System;

namespace IISManager.Implementations
{
    public class WorkerProcessDiagnosticValues : IEquatable<WorkerProcessDiagnosticValues>
    {
        private readonly double cpuUsage;
        private readonly double memoryUsage;

        public WorkerProcessDiagnosticValues(double cpuUsage, double memoryUsage)
        {
            this.cpuUsage = cpuUsage;
            this.memoryUsage = memoryUsage;
        }

        public string CpuUsage { get => $"{cpuUsage:F2}%"; }
        public string MemoryUsage { get => $"{memoryUsage:F2}MB"; }

        public bool Equals(WorkerProcessDiagnosticValues other)
        {
            return Math.Abs(this.cpuUsage - other.cpuUsage) < 0.01 &&
                Math.Abs(this.memoryUsage - other.memoryUsage) < 0.01;
        }
    }
}
