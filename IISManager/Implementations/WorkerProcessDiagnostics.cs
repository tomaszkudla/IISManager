using IISManager.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IISManager.Implementations
{
    public sealed class WorkerProcessDiagnostics
    {
        private static readonly WorkerProcessDiagnostics instance = new WorkerProcessDiagnostics();
        private readonly GenericMemoryCache<Tuple<DateTime, TimeSpan>> lastValues = new GenericMemoryCache<Tuple<DateTime, TimeSpan>>("CPU-USAGE-LAST-VALUES", TimeSpan.FromSeconds(10));
        private Dictionary<int, double> cpuUsagesByProcessId = new Dictionary<int, double>();
        private Dictionary<int, double> memoryUsagesByProcessId = new Dictionary<int, double>();

        static WorkerProcessDiagnostics()
        {
        }

        private WorkerProcessDiagnostics()
        {
        }

        public static WorkerProcessDiagnostics Instance
        {
            get
            {
                return instance;
            }
        }

        public WorkerProcessDiagnosticValues GetWorkerProcessDiagnosticValuesForProcessId(int processId)
        {
            cpuUsagesByProcessId.TryGetValue(processId, out var cpuUsage);
            memoryUsagesByProcessId.TryGetValue(processId, out var memoryUsage);
            return new WorkerProcessDiagnosticValues(cpuUsage, memoryUsage);
        }

        public void Refresh(IEnumerable<int> processIds)
        {
            var newCpuUsagesByProcessId = new Dictionary<int, double>();
            var newMemoryUsagesByProcessId = new Dictionary<int, double>();
            foreach (var id in processIds.Distinct())
            {
                var process = Process.GetProcessById(id);
                RefreshCpuUsage(newCpuUsagesByProcessId, id, process);
                RefreshMemoryUsage(newMemoryUsagesByProcessId, id, process);
            }

            cpuUsagesByProcessId = newCpuUsagesByProcessId;
            memoryUsagesByProcessId = newMemoryUsagesByProcessId;
        }

        private void RefreshCpuUsage(Dictionary<int, double> newCpuUsagesByProcessId, int id, Process process)
        {
            var curTime = DateTime.Now;
            var curTotalProcessorTime = process.TotalProcessorTime;
            var key = id.ToString();
            var lastValue = lastValues.GetItem(key);
            if (lastValue == null)
            {
                lastValues.Add(key, new Tuple<DateTime, TimeSpan>(curTime, curTotalProcessorTime));
                newCpuUsagesByProcessId.Add(id, 0.0);
            }
            else
            {
                var lastTime = lastValue.Item1;
                var lastTotalProcessorTime = lastValue.Item2;

                var cpuUsage = (curTotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds) / curTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount) * 100;
                newCpuUsagesByProcessId.Add(id, cpuUsage);
                lastValues.Add(key, new Tuple<DateTime, TimeSpan>(curTime, curTotalProcessorTime));
            }
        }

        private void RefreshMemoryUsage(Dictionary<int, double> newMemoryUsagesByProcessId, int id, Process process)
        {
            newMemoryUsagesByProcessId.Add(id, process.PrivateMemorySize64 / (double)1048576);
        }
    }
}
