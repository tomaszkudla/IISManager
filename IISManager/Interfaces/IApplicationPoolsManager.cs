using IISManager.Implementations;

namespace IISManager.Interfaces
{
    public interface IApplicationPoolsManager
    {
        ApplicationPoolsCollection ApplicationPools { get; }
        void Refresh();
    }
}
