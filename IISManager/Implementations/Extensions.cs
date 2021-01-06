using System.Collections.Generic;
using System.Linq;

namespace IISManager.Implementations
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
                    return applicationPools.OrderBy(p => p.State).ThenBy(p => p.Name).ToList();
                case SortingType.ByStateDsc:
                    return applicationPools.OrderByDescending(p => p.State).ThenBy(p => p.Name).ToList();
                default:
                    return applicationPools.ToList();
            }
        }
    }
}
