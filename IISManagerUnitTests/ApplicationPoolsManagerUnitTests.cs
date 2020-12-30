using Microsoft.VisualStudio.TestTools.UnitTesting;
using IISManager.Implementations;

namespace IISManagerUnitTests
{
    [TestClass]
    public class ApplicationPoolsManagerUnitTests
    {
        private readonly ApplicationPoolsManager manager;
        public ApplicationPoolsManagerUnitTests()
        {
            manager = ApplicationPoolsManager.Instance;
        }

        [TestMethod]
        public void Should_GetApplicationPools()
        {
            var applicationPools = manager.ApplicationPools;
            Assert.IsNotNull(applicationPools);
        }
    }
}
