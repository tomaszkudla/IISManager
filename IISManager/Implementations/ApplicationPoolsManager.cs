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

        public void Refresh()
        {
            //TODO refactor
            //var applicationPoolsDict = new ServerManager().ApplicationPools.ToDictionary(p => p.Name, p => p);
            //foreach (var applicationPool in applicationPoolsDict)
            //{
            //    var currentAppPool = applicationPools.Value.FirstOrDefault(p => p.Name == applicationPool.Value.Name);
            //    if (currentAppPool == null)
            //    {
            //        applicationPools.Value.Add(new ApplicationPool(applicationPool.Value));
            //    }
            //    else
            //    {
            //        applicationPools.Value.Remove(currentAppPool);
            //        applicationPools.Value.Add(new ApplicationPool(applicationPool.Value)
            //        {
            //            IsSelected = currentAppPool.IsSelected
            //        });
            //    }
            //}

            //foreach (var applicationPool in applicationPools.Value.ToList())
            //{
            //    if (!applicationPoolsDict.ContainsKey(applicationPool.Name))
            //    {
            //        applicationPools.Value.Remove(applicationPools.Value.FirstOrDefault(p => p.Name == applicationPool.Name));
            //    }
            //}

            var selectedAppPools = new HashSet<string>(applicationPools.Value.Where(p => p.IsSelected).Select(p => p.Name));
            applicationPools.Value = new ServerManager().ApplicationPools.Select(p => new ApplicationPool(p)
            {
                IsSelected = selectedAppPools.Contains(p.Name)
            }).ToList();
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
            applicationPools.Value.ToList().ForEach(p => p.IsSelected = true);
        }

        public void UnselectAll()
        {
            applicationPools.Value.ToList().ForEach(p => p.IsSelected = false);
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
