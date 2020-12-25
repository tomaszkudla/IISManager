using IISManager.Implementations;
using System.Collections.ObjectModel;

namespace IISManager.Interfaces
{
    public interface IApplicationPool
    {
        string Name { get; }
        ApplicationPoolState State { get; }
        WorkerProcessesCollection WorkerProcesses { get; }
        void Recycle();
        void Start();
        void Stop();
        void Refresh(Microsoft.Web.Administration.ApplicationPool applicationPool);
    }
}
