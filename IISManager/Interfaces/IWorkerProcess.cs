namespace IISManager.Interfaces
{
    public interface IWorkerProcess : IStoppable
    {
        string Name { get; set; }
        int Id { get; set; }
    }
}
