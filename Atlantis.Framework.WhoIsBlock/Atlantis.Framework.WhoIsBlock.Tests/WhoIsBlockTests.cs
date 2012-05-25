using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.WhoIsSaveBlock.Interface;
using Atlantis.Framework.WhoIsGetBlock.Interface;

namespace Atlantis.Framework.WhoIsBlock.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class WhoIsBlockTests
    {

        private string _ip = "192.168.0.1";
        private int _timespan = 2*3600;
        private string _page = "whois.aspx";


        public WhoIsBlockTests()
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

        #region WhoIsGetBlock

        [TestMethod]
        [DeploymentItem("atlantis.config")]
        public void WhoIsGetBlockPositive()
        {

            WhoIsGetBlockRequestData request = new WhoIsGetBlockRequestData("","","","",0,_ip, _timespan);
            var response = (WhoIsGetBlockResponseData)Engine.Engine.ProcessRequest(request, 321);

            Assert.IsTrue(response.IsSuccess);
        }

        // Negative tests not possible here.

        #endregion WhoIsGetBlock

        #region WhoIsSaveBlock
        [TestMethod]
        [DeploymentItem("atlantis.config")]
        public void WhoIsSaveBlockPositive()
        {

            WhoIsSaveBlockRequestData request = new WhoIsSaveBlockRequestData("", "", "", "", 0, _ip, _page);
            var response = (WhoIsSaveBlockResponseData)Engine.Engine.ProcessRequest(request, 322);

            Assert.IsTrue(response.IsSuccess);
        }

        // no negative test is possible here

        #endregion WhoIsSaveBlock
    }
}
