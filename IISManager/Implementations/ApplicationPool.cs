using IISManager.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace IISManager.Implementations
{
    public class ApplicationPool : IApplicationPool, INotifyPropertyChanged
    {
        private readonly WorkerProcessesCollection workerProcesses = new WorkerProcessesCollection();
        private Microsoft.Web.Administration.ApplicationPool applicationPool;

        public ApplicationPool(Microsoft.Web.Administration.ApplicationPool applicationPool)
        {
            Refresh(applicationPool);
        }

        public string Name { get => applicationPool.Name; }
        public ApplicationPoolState State { get => (ApplicationPoolState)(int)applicationPool.State; }

        public WorkerProcessesCollection WorkerProcesses => workerProcesses;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Recycle()
        {
            applicationPool.Recycle();
        }

        public void Refresh(Microsoft.Web.Administration.ApplicationPool applicationPool)
        {
            this.applicationPool = applicationPool;
            var workerProcessesList = GetWorkerProcesses();
            workerProcesses.Clear();
            workerProcessesList.ForEach(workerProcesses.Add);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WorkerProcesses"));
        }

        public void Start()
        {
            applicationPool.Start();
        }

        public void Stop()
        {
            applicationPool.Stop();
        }

        private List<WorkerProcess> GetWorkerProcesses()
        {
            return applicationPool.WorkerProcesses.Select(p => new WorkerProcess(p)).ToList();
        }
    }
}
