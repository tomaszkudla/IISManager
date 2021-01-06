using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IISManager.Implementations
{
    public sealed class CpuUsageCounters
    {
        private static readonly CpuUsageCounters instance = new CpuUsageCounters();
        private readonly GenericMemoryCache<Tuple<DateTime, TimeSpan>> lastValues = new GenericMemoryCache<Tuple<DateTime, TimeSpan>>("CPU-USAGE-LAST-VALUES", TimeSpan.FromSeconds(10));
        private Dictionary<int, double> cpuUsagesByProcessId = new Dictionary<int, double>();

        static CpuUsageCounters()
        {
        }

        private CpuUsageCounters()
        {
        }

        public static CpuUsageCounters Instance
        {
            get
            {
                return instance;
            }
        }

        public double GetCpuUsagesForProcessId(int processId)
        {
            if (cpuUsagesByProcessId.TryGetValue(processId, out var value))
            {
                return value;
            }

            return 0;
        }

        public void Refresh(IEnumerable<int> processIds)
        {
            var newCpuUsagesByProcessId = new Dictionary<int, double>();
            foreach (var id in processIds.Distinct())
            {
                var process = Process.GetProcessById(id);
                var key = id.ToString();
                var curTime = DateTime.Now;
                var curTotalProcessorTime = process.TotalProcessorTime;
                
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
                    lastValues.Add(key,new Tuple<DateTime, TimeSpan>(curTime, curTotalProcessorTime));
                }
            }

            cpuUsagesByProcessId = newCpuUsagesByProcessId;
        }
    }
}
