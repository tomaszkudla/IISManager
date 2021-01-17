using System;

namespace IISManager.Implementations
{
    public class WorkerProcessDiagnosticValues : IEquatable<WorkerProcessDiagnosticValues>
    {
        public WorkerProcessDiagnosticValues(double cpuUsage, double memoryUsage)
        {
            CpuUsage = new DiagnosticValue(cpuUsage, 10.0, 30.0, "{0:F2}%");
            MemoryUsage = new DiagnosticValue(memoryUsage, 10.0, 100.0, "{0:F2}MB");
        }

        public DiagnosticValue CpuUsage { get; }
        public DiagnosticValue MemoryUsage { get; }

        public bool Equals(WorkerProcessDiagnosticValues other)
        {
            return this.CpuUsage == other.CpuUsage &&
                this.MemoryUsage == other.MemoryUsage;
        }
    }
}
