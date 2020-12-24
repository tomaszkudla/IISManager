using IISManager.Interfaces;
using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.Linq;

namespace IISManager.Implementations
{
    public class ApplicationPoolsManager : IApplicationPoolsManager
    {
        private readonly ServerManager serverManager;
        
        public ApplicationPoolsManager()
        {
            serverManager = new ServerManager();
        }

        public List<ApplicationPool> GetApplicationPools()
        {
            return serverManager.ApplicationPools.Select(p => new ApplicationPool(p)).ToList();
        }
    }
}
