using IISManager.Implementations;

namespace IISManager.Interfaces
{
    public interface IWorkerProcess
    {
        int Id { get; }
        WorkerProcessState State { get; }
        float CpuUsage { get; }
    }
}
