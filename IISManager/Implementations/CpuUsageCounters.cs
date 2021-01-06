using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IISManager.Implementations
{
    public sealed class CpuUsageCounters
    {
        const string w3wpProcessName = "w3wp";
        private static readonly CpuUsageCounters instance = new CpuUsageCounters();
        private readonly GenericMemoryCache<PerformanceCounter> cpuUsageCountersCache = new GenericMemoryCache<PerformanceCounter>("CPU-USAGE-COUNTERS", TimeSpan.FromMinutes(5));

        private readonly Dictionary<int, Tuple<DateTime, TimeSpan>> lastValues = new Dictionary<int, Tuple<DateTime, TimeSpan>>();

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

        public Dictionary<int, double> GetCpuUsages(IEnumerable<int> processIds)
        {
            var sw = Stopwatch.StartNew();
            var result = new Dictionary<int, double>();
            foreach (var id in processIds)
            {
                var process = Process.GetProcessById(id);
                if (lastValues.TryGetValue(id, out var lastValue))
                {
                    var curTime = DateTime.Now;
                    var curTotalProcessorTime = process.TotalProcessorTime;

                    var lastTime = lastValue.Item1;
                    var lastTotalProcessorTime = lastValue.Item2;

                    var cpuUsage = (curTotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds) / curTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);
                    result.Add(id, cpuUsage);
                    lastValues[id] = new Tuple<DateTime, TimeSpan>(curTime, curTotalProcessorTime);
                }
                else
                {
                    lastValues.Add(id, new Tuple<DateTime, TimeSpan>(DateTime.Now, process.TotalProcessorTime));
                    result.Add(id, 0.0);
                }
            }

            sw.Stop();
            if (result.Any(r => r.Value > 0.01))
            {

                var test = sw.Elapsed;
            }
            return result;
        }
    }
}
