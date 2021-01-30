using Microsoft.VisualStudio.TestTools.UnitTesting;
using IISManager.Implementations;
using Microsoft.Extensions.Logging.Abstractions;

namespace IISManagerUnitTests
{
    [TestClass]
    public class ApplicationPoolsManagerUnitTests
    {
        private readonly WorkerProcessDiagnostics workerProcessDiagnostics;
        private readonly ApplicationPoolsManager manager;
        public ApplicationPoolsManagerUnitTests()
        {
            workerProcessDiagnostics = new WorkerProcessDiagnostics(NullLoggerFactory.Instance);
            manager = new ApplicationPoolsManager(NullLoggerFactory.Instance, workerProcessDiagnostics);
        }

        [TestMethod]
        public void Should_GetApplicationPools()
        {
            var applicationPools = manager.ApplicationPools;
            Assert.IsNotNull(applicationPools);
        }
    }
}
