using IISManager.Implementations;
using System.Collections.Generic;

namespace IISManager.Interfaces
{
    public interface IApplicationPool
    {
        string Name { get; }
        ApplicationPoolState State { get; }
        List<WorkerProcess> WorkerProcesses { get; }
        void Recycle();
        void Start();
        void Stop();
    }
}
