using Microsoft.VisualStudio.TestTools.UnitTesting;
using IISManager.Implementations;
using Microsoft.Extensions.Logging.Abstractions;

namespace IISManagerUnitTests
{
    [TestClass]
    public class ApplicationPoolsManagerUnitTests
    {
        private readonly CurrentProcessWrapper currentProcessWrapper;
        private readonly ProcessDiagnostics processDiagnostics;
        private readonly ApplicationPoolsManager manager;
        public ApplicationPoolsManagerUnitTests()
        {
            currentProcessWrapper = new CurrentProcessWrapper();
            processDiagnostics = new ProcessDiagnostics(NullLoggerFactory.Instance, currentProcessWrapper);
            manager = new ApplicationPoolsManager(NullLoggerFactory.Instance, processDiagnostics);
        }

        [TestMethod]
        public void Should_GetApplicationPools()
        {
            var applicationPools = manager.ApplicationPools;
            Assert.IsNotNull(applicationPools);
        }
    }
}
