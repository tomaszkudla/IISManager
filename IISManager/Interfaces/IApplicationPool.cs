using IISManager.Implementations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IISManager.Interfaces
{
    public interface IApplicationPool
    {
        bool IsSelected { get; set; }
        string Name { get; }
        ApplicationPoolState State { get; }
        ObservableList<WorkerProcess> WorkerProcesses { get; }
        void Recycle();
        void Start();
        void Stop();
    }
}
