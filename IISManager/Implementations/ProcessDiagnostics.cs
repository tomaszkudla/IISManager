using IISManager.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IISManager.Implementations
{
    public sealed class ProcessDiagnostics
    {
        private readonly ILogger<ProcessDiagnostics> logger;
        private readonly Process currentProcess;
        private readonly GenericMemoryCache<Tuple<DateTime, TimeSpan>> lastValues = new GenericMemoryCache<Tuple<DateTime, TimeSpan>>("CPU-USAGE-LAST-VALUES", TimeSpan.FromSeconds(10));
        private Dictionary<int, double> cpuUsagesByProcessId = new Dictionary<int, double>();
        private Dictionary<int, double> memoryUsagesByProcessId = new Dictionary<int, double>();

        public ProcessDiagnostics(ILoggerFactory loggerFactory, CurrentProcessWrapper currentProcessWrapper)
        {
            logger = loggerFactory.CreateLogger<ProcessDiagnostics>();
            currentProcess = currentProcessWrapper.CurrentProcess;
        }

        public ProcessDiagnosticValues GetProcessDiagnosticValuesByProcessId(int processId)
        {
            cpuUsagesByProcessId.TryGetValue(processId, out var cpuUsage);
            memoryUsagesByProcessId.TryGetValue(processId, out var memoryUsage);
            return new ProcessDiagnosticValues(cpuUsage, memoryUsage);
        }

        public ProcessDiagnosticValues GetProcessDiagnosticValuesForCurrentProcess()
        {
            cpuUsagesByProcessId.TryGetValue(currentProcess.Id, out var cpuUsage);
            memoryUsagesByProcessId.TryGetValue(currentProcess.Id, out var memoryUsage);
            return new ProcessDiagnosticValues(cpuUsage, memoryUsage);
        }

        public void Refresh(IEnumerable<int> processIds)
        {
            var newCpuUsagesByProcessId = new Dictionary<int, double>();
            var newMemoryUsagesByProcessId = new Dictionary<int, double>();

            RefreshProcessesUsages(processIds, newCpuUsagesByProcessId, newMemoryUsagesByProcessId);
            RefreshCurrentProcessUsages(newCpuUsagesByProcessId, newMemoryUsagesByProcessId);

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

        private void RefreshProcessesUsages(IEnumerable<int> processIds, Dictionary<int, double> newCpuUsagesByProcessId, Dictionary<int, double> newMemoryUsagesByProcessId)
        {
            foreach (var id in processIds.Distinct())
            {
                try
                {
                    var process = Process.GetProcessById(id);
                    RefreshCpuUsage(newCpuUsagesByProcessId, id, process);
                    RefreshMemoryUsage(newMemoryUsagesByProcessId, id, process);
                }
                catch
                {
                }
            }
        }

        private void RefreshCurrentProcessUsages(Dictionary<int, double> newCpuUsagesByProcessId, Dictionary<int, double> newMemoryUsagesByProcessId)
        {
            RefreshCpuUsage(newCpuUsagesByProcessId, currentProcess.Id, currentProcess);
            RefreshMemoryUsage(newMemoryUsagesByProcessId, currentProcess.Id, currentProcess);
        }
    }
}
