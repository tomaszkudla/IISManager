using IISManager.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace IISManager.Utils
{
    public static class Extensions
    {
        public static List<ApplicationPool> OrderAppPoolsBy(this IEnumerable<ApplicationPool> applicationPools, SortingType sortingType)
        {
            switch (sortingType)
            {
                case SortingType.ByNameAsc:
                    return applicationPools.OrderBy(p => p.Name).ToList();
                case SortingType.ByNameDsc:
                    return applicationPools.OrderByDescending(p => p.Name).ToList();
                case SortingType.ByStateAsc:
                    return applicationPools.OrderBy(p => p.State).ThenBy(p => !p.WorkerProcesses.Value.Any()).ThenBy(p => p.Name).ToList();
                case SortingType.ByStateDsc:
                    return applicationPools.OrderByDescending(p => p.State).ThenBy(p => p.WorkerProcesses.Value.Any()).ThenBy(p => p.Name).ToList();
                case SortingType.ByCpuUsageAsc:
                    return applicationPools.OrderBy(p => p.WorkerProcesses.Value.Sum(wp => wp.ProcessDiagnosticValues.CpuUsage.Value)).ThenByDescending(p => p.State).ThenBy(p => p.WorkerProcesses.Value.Any()).ThenBy(p => p.Name).ToList();
                case SortingType.ByCpuUsageDsc:
                    return applicationPools.OrderByDescending(p => p.WorkerProcesses.Value.Sum(wp => wp.ProcessDiagnosticValues.CpuUsage.Value)).ThenBy(p => p.State).ThenBy(p => !p.WorkerProcesses.Value.Any()).ThenBy(p => p.Name).ToList();
                default:
                    return applicationPools.ToList();
            }
        }

        public static List<ApplicationPool> FilterAppPools(this IEnumerable<ApplicationPool> applicationPools, string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return applicationPools.ToList();
            }

            return applicationPools.Where(p => p.Name.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }
    }
}
