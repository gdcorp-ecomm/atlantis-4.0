using Atlantis.Framework.ProximaDomainName.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ProximaDomainName.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ProximaDomainNameTests
    {
        public ProximaDomainNameTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [DeploymentItem("atlantis.config")]
        public void ProximaDomainNameIsDeluxe()
        {
            ProximaDomainNameResponseData response = null;
            string domain = "samanthamyers.info";
            ProximaDomainNameRequestData request = new ProximaDomainNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, domain, 3000);
            response = (ProximaDomainNameResponseData)Engine.Engine.ProcessRequest(request, 401);

            Assert.IsTrue(response.IsDeluxe);
        }

        [TestMethod]
        [DeploymentItem("atlantis.config")]
        public void ProximaDomainNameIsNotDeluxe()
        {
            ProximaDomainNameResponseData response = null;
            string domain = "mydogsbutt.com";
            ProximaDomainNameRequestData request = new ProximaDomainNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, domain, 3000);
            response = (ProximaDomainNameResponseData)Engine.Engine.ProcessRequest(request, 401);

            Assert.IsTrue(!response.IsDeluxe);
        }
    }
}