using Microsoft.VisualStudio.TestTools.UnitTesting;
using IISManager.Implementations;
using Microsoft.Extensions.Logging.Abstractions;
using IISManager.ViewModels;

namespace IISManagerUnitTests
{
    [TestClass]
    public class ApplicationPoolsManagerUnitTests
    {
        private readonly CurrentProcessWrapper currentProcessWrapper;
        private readonly ProcessDiagnostics processDiagnostics;
        private readonly IISServerManager iisServerManager;
        private readonly UserMessage userMessage;
        private readonly ApplicationPoolsManager manager;

        public ApplicationPoolsManagerUnitTests()
        {
            currentProcessWrapper = new CurrentProcessWrapper();
            processDiagnostics = new ProcessDiagnostics(NullLoggerFactory.Instance, currentProcessWrapper);
            userMessage = new UserMessage();
            iisServerManager = new IISServerManager(NullLoggerFactory.Instance, userMessage);
            manager = new ApplicationPoolsManager(NullLoggerFactory.Instance, processDiagnostics, iisServerManager, userMessage);
        }

        [TestMethod]
        public void Should_GetApplicationPools()
        {
            var applicationPools = manager.ApplicationPools;
            Assert.IsNotNull(applicationPools);
        }
    }
}
