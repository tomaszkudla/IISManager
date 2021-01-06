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
            var processNamesForIds = GetProcessNamesForIds(processIds);
            var result = new Dictionary<int, float>();
            foreach (var id in processIds.Distinct())
            {
                //var cachedCounter = cpuUsageCountersCache.GetItem(id.ToString());
                //if (cachedCounter == null)
                //{

                //}
                //else
                //{
                //    result.Add(id, cachedCounter.NextValue());
                //}

            }

            return result;
        }

        private Dictionary<int, string> GetProcessNamesForIds(IEnumerable<int> processIds)
        {
            var w3wpProcessesCount = Process.GetProcessesByName(w3wpProcessName).Length;
            var w3wpProcessesNames = GenerateW3wpProcessesNames(w3wpProcessesCount);
            var idsAndNamesLookup = w3wpProcessesNames.ToLookup(n => new PerformanceCounter("Process", "ID Process", n, true).NextValue(), n => n).Distinct();
            var result = new Dictionary<int, string>();
            foreach (var idAndName in idsAndNamesLookup)
            {
                var id = (int)idAndName.Key;
                var name = idAndName.FirstOrDefault();
                if (!result.ContainsKey(id))
                {
                    result.Add(id, name);
                }
            }

            return result;
        }

        private IEnumerable<string> GenerateW3wpProcessesNames(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    yield return w3wpProcessName;
                }
                else
                {
                    yield return $"{w3wpProcessName}#{i}";
                }
            }
        }
    }
}
