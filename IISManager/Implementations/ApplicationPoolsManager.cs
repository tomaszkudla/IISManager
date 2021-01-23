using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IISManager.Implementations
{
    public sealed class ApplicationPoolsManager
    {
        private static readonly ApplicationPoolsManager instance = new ApplicationPoolsManager();
        private readonly ApplicationPoolsList applicationPools = new ApplicationPoolsList();

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

        public ApplicationPoolsList ApplicationPools
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
            using (var serverManager = new ServerManager())
            {
                WorkerProcessDiagnostics.Instance.Refresh(serverManager.WorkerProcesses.Select(p => p.ProcessId));
                var appPoolsRaw = serverManager.ApplicationPools.Select(p => new ApplicationPool(p));
                applicationPools.Value = appPoolsRaw.FilterAppPools(Filter.Value).OrderAppPoolsBy(Sorting.Value);
            }
        }

        public void Select(string name)
        {
            applicationPools.SelectedApplicationPools.Add(name);
        }

        public void Unselect(string name)
        {
            applicationPools.SelectedApplicationPools.Remove(name);
            AllSelected.Value = false;
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
