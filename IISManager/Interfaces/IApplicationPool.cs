using IISManager.Implementations;
using System.Collections.ObjectModel;

namespace IISManager.Interfaces
{
    public interface IApplicationPool
    {
        string Name { get; }
        ApplicationPoolState State { get; }
        ObservableCollection<WorkerProcess> WorkerProcesses { get; }
        void Recycle();
        void Start();
        void Stop();
    }
}
