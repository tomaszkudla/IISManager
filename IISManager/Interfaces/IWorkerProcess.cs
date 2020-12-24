using IISManager.Implementations;

namespace IISManager.Interfaces
{
    public interface IWorkerProcess
    {
        string Name { get; }
        int Id { get; }
        WorkerProcessState State { get; }
    }
}
