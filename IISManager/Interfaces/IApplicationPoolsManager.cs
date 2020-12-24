using IISManager.Implementations;
using System.Collections.Generic;

namespace IISManager.Interfaces
{
    public interface IApplicationPoolsManager
    {
        List<ApplicationPool> GetApplicationPools();
    }
}
