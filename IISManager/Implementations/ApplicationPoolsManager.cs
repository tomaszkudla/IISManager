using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IISManager.Implementations
{
    public class ApplicationPoolsManager : IApplicationPoolsManager
    {
        private readonly ServerManager serverManager;
        private readonly ObservableCollection<ApplicationPool> applicationPools = new ObservableCollection<ApplicationPool>();
        public ApplicationPoolsManager()
        {
            serverManager = new ServerManager();
        }

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
                    applicationPools.Add(new ApplicationPool(applicationPool.Value));
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

        private List<ApplicationPool> GetApplicationPools()
        {
            return serverManager.ApplicationPools.Select(p => new ApplicationPool(p)).ToList();
        }
    }
}
