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
        private readonly ApplicationPoolsCollection applicationPools = new ApplicationPoolsCollection();
        public ApplicationPoolsManager()
        {
            serverManager = new ServerManager();
        }

        public ApplicationPoolsCollection ApplicationPools
        {
            get
            {
                Refresh();
                return applicationPools;
            }
        }

        public void Refresh()
        {
            var applicationPoolsDict = serverManager.ApplicationPools.ToDictionary(p => p.Name, p => p);
            foreach (var applicationPool in applicationPoolsDict)
            {
                var currentAppPool = applicationPools.FirstOrDefault(p => p.Name == applicationPool.Value.Name);
                if (currentAppPool == null)
                {
                    applicationPools.Add(new ApplicationPool(applicationPool.Value));
                }
                else
                {
                    currentAppPool.Refresh(applicationPool.Value);
                }
            }

            foreach (var applicationPool in applicationPools.ToList())
            {
                if (!applicationPoolsDict.ContainsKey(applicationPool.Name))
                {
                    applicationPools.Remove(applicationPools.FirstOrDefault(p => p.Name == applicationPool.Name));
                }
            }

            applicationPools.Add(new ApplicationPool(applicationPoolsDict.First().Value));

            applicationPools.Clear();
            serverManager.ApplicationPools.ToList().ForEach(p => applicationPools.Add(new ApplicationPool(p)));
            //applicationPools.Clear();
            //GetApplicationPools().ForEach(applicationPools.Add);
            applicationPools.Refresh();
        }

        private List<ApplicationPool> GetApplicationPools()
        {
            return serverManager.ApplicationPools.Select(p => new ApplicationPool(p)).ToList();
        }
    }
}
