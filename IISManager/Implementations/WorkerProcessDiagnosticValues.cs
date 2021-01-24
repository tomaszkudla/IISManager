namespace IISManager.Implementations
{
    public class WorkerProcessDiagnosticValues
    {
        public WorkerProcessDiagnosticValues(double cpuUsage, double memoryUsage)
        {
            CpuUsage = new DiagnosticValue(cpuUsage, "{0:F2}%");
            MemoryUsage = new DiagnosticValue(memoryUsage, "{0:F2}MB");
        }

        public DiagnosticValue CpuUsage { get; }
        public DiagnosticValue MemoryUsage { get; }
    }
}
