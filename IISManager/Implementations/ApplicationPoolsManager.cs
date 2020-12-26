﻿using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System.Collections.ObjectModel;
using System.Linq;

namespace IISManager.Implementations
{
    public class ApplicationPoolsManager : IApplicationPoolsManager
    {
        private readonly ObservableCollection<ApplicationPool> applicationPools = new ObservableCollection<ApplicationPool>();

        public ObservableCollection<ApplicationPool> ApplicationPools
        {
            get
            {
                Refresh();
                return applicationPools;
            }
        }

        public void Refresh()
        {
            //TODO refactor
            var applicationPoolsDict = new ServerManager().ApplicationPools.ToDictionary(p => p.Name, p => p);
            foreach (var applicationPool in applicationPoolsDict)
            {
                var currentAppPool = applicationPools.FirstOrDefault(p => p.Name == applicationPool.Value.Name);
                if (currentAppPool == null)
                {
                    applicationPools.Add(new ApplicationPool(applicationPool.Value));
                }
                else
                {
                    applicationPools.Remove(currentAppPool);
                    applicationPools.Add(new ApplicationPool(applicationPool.Value)
                    {
                        IsSelected = currentAppPool.IsSelected
                    });
                }
            }

            foreach (var applicationPool in applicationPools.ToList())
            {
                if (!applicationPoolsDict.ContainsKey(applicationPool.Name))
                {
                    applicationPools.Remove(applicationPools.FirstOrDefault(p => p.Name == applicationPool.Name));
                }
            }
        }

        public void Select(string name)
        {
            var appPool = applicationPools.FirstOrDefault(p => p.Name == name);
            if (appPool != null)
            {
                appPool.IsSelected = true;
            }
        }

        public void Unselect(string name)
        {
            var appPool = applicationPools.FirstOrDefault(p => p.Name == name);
            if (appPool != null)
            {
                appPool.IsSelected = false;
            }
        }

        public void SelectAll()
        {
            applicationPools.ToList().ForEach(p => p.IsSelected = true);
        }

        public void UnselectAll()
        {
            applicationPools.ToList().ForEach(p => p.IsSelected = false);
        }

        public void StartSelected()
        {
            var selected = applicationPools.Where(p => p.IsSelected);
            foreach (var appPool in selected)
            {
                appPool.Start();
            }
        }

        public void StopSelected()
        {
            var selected = applicationPools.Where(p => p.IsSelected);
            foreach (var appPool in selected)
            {
                appPool.Stop();
            }
        }
    }
}
