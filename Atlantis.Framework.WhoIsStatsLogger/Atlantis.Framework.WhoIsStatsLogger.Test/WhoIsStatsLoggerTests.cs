using System;
using System.Data;
using System.Diagnostics;
using Atlantis.Framework.WhoIsStatsLogger.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsStatsLogger.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class WhoIsStatsLoggerTests
    {
    private const string _shopperId = "822497";
    private const int _resourceID = 406914;
    private const int _WhoIsStatsLoggerRequest = 320;

    public WhoIsStatsLoggerTests()
    {
    }

    private TestContext testContextInstance;

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

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void WebServiceResponseTest()
    {
      
      var _request = new WhoIsStatsLoggerRequestData("356941","", "","",0, "c1wsdv-rphil", "Windows", "Safari", "www.godaddy.com", "net", 0, DateTime.Now, DateTime.Now, 100, 8, false, false,false,false, "400");
      var _response = (WhoIsStatsLoggerResponseData)Engine.Engine.ProcessRequest(_request, _WhoIsStatsLoggerRequest);

      Assert.IsTrue(_response.IsSuccess);
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
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }
    }
}
