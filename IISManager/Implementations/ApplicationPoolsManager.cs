using IISManager.Utils;
using IISManager.ViewModels;
using Microsoft.Web.Administration;
using System.Collections.Generic;
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

        public void Refresh()
        {
            using (var serverManager = new ServerManager())
            {
                WorkerProcessDiagnostics.Instance.Refresh(serverManager.WorkerProcesses.Select(p => p.ProcessId));
                applicationPools.Value = serverManager.ApplicationPools.Select(p => new ViewModels.ApplicationPool(p)).ToList();
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
            applicationPools.SelectedApplicationPools = new HashSet<string>(applicationPools.Value.Select(p => p.Name));
        }

        public void UnselectAll()
        {
            applicationPools.SelectedApplicationPools = new HashSet<string>();
        }

        public void StartSelected()
        {
            var selectedPoolNames = applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name);
            using (var serverManager = new ServerManager())
            {
                var allPools = serverManager.ApplicationPools;
                var selectedPools = selectedPoolNames.Select(n => allPools.FirstOrDefault(p => p.Name == n)).Where(n => n != null);
                foreach (var appPool in selectedPools)
                {
                    if (appPool.State != ObjectState.Started)
                    {
                        appPool.Start();
                    }
                }
            }
        }

        public void StopSelected()
        {
            var selectedPoolNames = applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name);
            using (var serverManager = new ServerManager())
            {
                var allPools = serverManager.ApplicationPools;
                var selectedPools = selectedPoolNames.Select(n => allPools.FirstOrDefault(p => p.Name == n)).Where(n => n != null);
                foreach (var appPool in selectedPools)
                {
                    if (appPool.State != ObjectState.Stopped)
                    {
                        appPool.Stop();
                    }
                }
            }
        }

        public void RecycleSelected()
        {
            var selectedPoolNames = applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name);
            using (var serverManager = new ServerManager())
            {
                var allPools = serverManager.ApplicationPools;
                var selectedPools = selectedPoolNames.Select(n => allPools.FirstOrDefault(p => p.Name == n)).Where(n => n != null);
                foreach (var appPool in selectedPools)
                {
                    if (appPool.State != ObjectState.Stopped)
                    {
                        appPool.Recycle();
                    }
                }
            }
        }
    }
}
