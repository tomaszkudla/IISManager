using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.ComponentModel;
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
            Sorting.PropertyChanged += RefreshFiltering;
            Filter.PropertyChanged += RefreshFiltering;
            Refresh();
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
                return applicationPools;
            }
        }

        public Observable<bool> AllSelected { get; } = new Observable<bool>();
        public Observable<SortingType> Sorting { get; } = new Observable<SortingType>(SortingType.ByNameAsc);
        public Observable<string> Filter { get; } = new Observable<string>(string.Empty);

        public void Refresh()
        {
            var selectedAppPools = new HashSet<string>(applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name));
            using (var serverManager = new ServerManager())
            {
                var appPoolsRaw = serverManager.ApplicationPools.Select(p => new ApplicationPool(p)
                {
                    IsSelected = selectedAppPools.Contains(p.Name)
                });

                applicationPools.Value = appPoolsRaw.FilterAppPools(Filter.Value).OrderAppPoolsBy(Sorting.Value);

                CpuUsageCounters.Instance.GetCpuUsages(serverManager.WorkerProcesses.Select(p => p.ProcessId));
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
        
        private void RefreshFiltering(object sender, PropertyChangedEventArgs e)
        {
            ApplicationPools.Value = ApplicationPools.Value.FilterAppPools(Filter.Value).OrderAppPoolsBy(Sorting.Value);
        }
    }
}
