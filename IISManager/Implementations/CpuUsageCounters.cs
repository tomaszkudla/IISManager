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
        private readonly GenericMemoryCache<PerformanceCounter> processCountersCache = new GenericMemoryCache<PerformanceCounter>("CPU-USAGE-COUNTERS", TimeSpan.FromMinutes(5));

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

        public Dictionary<int, float> GetCpuUsages(IEnumerable<int> processIds)
        {
            var sw = Stopwatch.StartNew();
            var processIdsSet = new HashSet<int>(processIds.Distinct());
            var processNamesForIds = GetProcessNamesForIds(processIdsSet);
            var result = new Dictionary<int, float>();
            foreach (var id in processIdsSet)
            {
                if (processNamesForIds.TryGetValue(id, out var name))
                {
                    var counter = new PerformanceCounter("Process", "% Processor Time", name, true);

                    result.Add(id, counter.NextValue());

                }
            }

            sw.Stop();
            var test = sw.Elapsed;

            if (result.Any(r => r.Value > (float)0.00001))
            {
                test = sw.Elapsed;
            }
            return result;
        }

        private Dictionary<int, string> GetProcessNamesForIds(HashSet<int> processIds)
        {
            var sw = Stopwatch.StartNew();
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");

            var names = cat.GetInstanceNames().Where(n => n.Contains(w3wpProcessName));
            var result = new Dictionary<int, string>();
            foreach (var name in names)
            {
                var counter = new PerformanceCounter("Process", "ID Process", name, true);

                int id = (int)counter.RawValue;
                if (processIds.Contains(id))
                {
                    result.Add(id, name);
                }

            }
            sw.Stop();
            var test = sw.Elapsed;
            return result;
        }
    }
}
