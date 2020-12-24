using IISManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISManager.Implementations
{
    public class WorkerProcess : IWorkerProcess
    {
        public WorkerProcess(Microsoft.Web.Administration.WorkerProcess workerProcess)
        {
            Name = workerProcess.AppPoolName;
            Id = workerProcess.ProcessId;
        }

        public string Name { get; set; }
        public int Id { get; set; }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
