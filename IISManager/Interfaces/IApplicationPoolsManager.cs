using IISManager.Implementations;
using System.Collections.ObjectModel;

namespace IISManager.Interfaces
{
    public interface IApplicationPoolsManager
    {
        ObservableCollection<ApplicationPool> ApplicationPools { get; }
        void Refresh();
    }
}
