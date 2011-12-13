using Atlantis.Framework.CRMTaskCreation.Interface;
using Atlantis.Framework.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Framework.CrmTaskCreation.Test
{


    /// <summary>
    ///This is a test class for CrmTaskCreationRequestTestTest and is intended
    ///to contain all CrmTaskCreationRequestTestTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CrmTaskCreationRequestTestTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod]
        [DeploymentItem("atlantis.config")]
        public void CreateTask()
        {
            var request = new CRMTaskCreationRequestData("850774", string.Empty, "1452305",
                                                                                        string.Empty,
                                                                                        0, DateTime.Now, DateTime.Now.AddHours(1), 11);
            request.RequestTimeout = TimeSpan.FromSeconds(180);
            var response = (CRMTaskCreationResponseData)Engine.ProcessRequest(request, 295);

            Assert.IsTrue(response.IsSuccess);
        }
    }
}
