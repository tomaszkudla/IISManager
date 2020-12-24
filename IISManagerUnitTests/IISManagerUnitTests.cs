using Microsoft.VisualStudio.TestTools.UnitTesting;
using IISManager.Implementations;

namespace IISManagerUnitTests
{
    [TestClass]
    public class IISManagerUnitTests
    {
        private readonly WorkerProcessesManager manager;
        public IISManagerUnitTests()
        {
            manager = new WorkerProcessesManager();
        }

        [TestMethod]
        public void Should_GetWorkerProecesses()
        {
            var workerProcesses = manager.GetWorkerProcesses();
            Assert.IsNotNull(workerProcesses);
        }
    }
}
