using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.HCCAvailableSSL.Impl;
using Atlantis.Framework.HCCAvailableSSL.Interface;
using Atlantis.Framework.Testing.MockHttpContext;


namespace Atlantis.Framework.HCCAvailableSSL.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHCCAvailableSSLTests
  {
  
    private const string _shopperId = "";
	
	
    public GetHCCAvailableSSLTests()
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
    public void HCCAvailableSSLTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      HCCAvailableSSLRequestData request = new HCCAvailableSSLRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "e4ca712d-1e92-4aeb-a182-6439464ef879");

      //HCCAvailableSSLResponseData response = (HCCAvailableSSLResponseData)Engine.Engine.ProcessRequest(request, 275);

      var response = SessionCache.SessionCache.GetProcessRequest<HCCAvailableSSLResponseData>(request, 275);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
