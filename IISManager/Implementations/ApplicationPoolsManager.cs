using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.Linq;

namespace IISManager.Implementations
{
    public sealed class ApplicationPoolsManager : IApplicationPoolsManager
    {
        private static readonly ApplicationPoolsManager instance = new ApplicationPoolsManager();
        private readonly ObservableList<ApplicationPool> applicationPools = new ObservableList<ApplicationPool>();

        static ApplicationPoolsManager()
        {
        }

        private ApplicationPoolsManager()
        {
        }

        public static ApplicationPoolsManager Instance
        {
            get
            {
                return instance;
            }
        }

        public ObservableList<ApplicationPool> ApplicationPools
        {
            get
            {
                Refresh();
                return applicationPools;
            }
        }

        public Observable<bool> AllSelected { get; } = new Observable<bool>();
        public Observable<SortingType> Sorting { get; } = new Observable<SortingType>(SortingType.ByNameAsc);

        public void Refresh()
        {
            var selectedAppPools = new HashSet<string>(applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name));
            using (var serverManager = new ServerManager())
            {
                var appPoolsUnordered = serverManager.ApplicationPools.Select(p => new ApplicationPool(p)
                {
                    IsSelected = selectedAppPools.Contains(p.Name)
                });

                switch (Sorting.Value)
                {
                    case SortingType.ByNameAsc:
                        applicationPools.Value = appPoolsUnordered.OrderBy(p => p.Name).ToList();
                        break;
                    case SortingType.ByNameDsc:
                        applicationPools.Value = appPoolsUnordered.OrderByDescending(p => p.Name).ToList();
                        break;
                    case SortingType.ByStateAsc:
                        applicationPools.Value = appPoolsUnordered.OrderBy(p => p.State).ThenBy(p => p.Name).ToList();
                        break;
                    case SortingType.ByStateDsc:
                        applicationPools.Value = appPoolsUnordered.OrderByDescending(p => p.State).ThenBy(p => p.Name).ToList();
                        break;
                    default:
                        applicationPools.Value = appPoolsUnordered.ToList();
                        break;
                }
            }
        }

        public void Select(string name)
        {
            var appPool = applicationPools.Value.FirstOrDefault(p => p.Name == name);
            if (appPool != null)
            {
                appPool.IsSelected = true;
            }
        }

        public void Unselect(string name)
        {
            var appPool = applicationPools.Value.FirstOrDefault(p => p.Name == name);
            if (appPool != null)
            {
                appPool.IsSelected = false;
                AllSelected.Value = false;
            }
        }

        public void SelectAll()
        {
            var appPools = applicationPools.Value.ConvertAll(p => p.Clone());
            appPools.ForEach(p => p.IsSelected = true);
            applicationPools.Value = appPools;
        }

        public void UnselectAll()
        {
            var appPools = applicationPools.Value.ConvertAll(p => p.Clone());
            appPools.ForEach(p => p.IsSelected = false);
            applicationPools.Value = appPools;
        }

        public void StartSelected()
        {
            var selected = applicationPools.Value.Where(p => p.IsSelected);
            foreach (var appPool in selected)
            {
                appPool.Start();
            }
        }

        public void StopSelected()
        {
            var selected = applicationPools.Value.Where(p => p.IsSelected);
            foreach (var appPool in selected)
            {
                appPool.Stop();
            }
        }

        public void RecycleSelected()
        {
            var selected = applicationPools.Value.Where(p => p.IsSelected);
            foreach (var appPool in selected)
            {
                appPool.Recycle();
            }
        }
    }
}
