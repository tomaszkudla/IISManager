using Microsoft.VisualStudio.TestTools.UnitTesting;
using IISManager.Implementations;

namespace IISManagerUnitTests
{
    [TestClass]
    public class IISManagerUnitTests
    {
        private readonly ApplicationPoolsManager manager;
        public IISManagerUnitTests()
        {
            manager = new ApplicationPoolsManager();
        }

        [TestMethod]
        public void Should_GetApplicationPools()
        {
            var applicationPools = manager.GetApplicationPools();
            Assert.IsNotNull(applicationPools);
        }
    }
}
