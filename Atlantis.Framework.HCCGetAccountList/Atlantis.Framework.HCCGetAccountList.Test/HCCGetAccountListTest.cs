using System.Diagnostics;
using Atlantis.Framework.HCCGetAccountList.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HCCGetAccountList.Test
{
  /// <summary>
  /// Summary description for Atlantis
  /// </summary>
  [TestClass]
  public class HCCGetAccountListTest
  {
    public HCCGetAccountListTest()
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
    public void HCCGetAccountListResponseTest()
    {
      HCCGetAccountListRequestData request = new HCCGetAccountListRequestData("12530",
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      HCCGetAccountListResponseData responseData = Engine.Engine.ProcessRequest(request, 256) as HCCGetAccountListResponseData;
      
      //Assert.IsTrue(responseData.IsSuccess);
      //Assert.AreNotEqual("", responseData.ToXML());
      Assert.IsTrue(responseData.ToXML().Contains("accounts"));

      Debug.WriteLineIf(responseData != null, responseData.ToXML());
      //Assert.IsTrue(responseData != null && responseData.HCCResponse != null && responseData.HCCResponse.AccountList.Count > 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HCCGetAccountListResponseTestFromSession()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      HCCGetAccountListRequestData request = new HCCGetAccountListRequestData("12530",
        string.Empty,
        string.Empty,
        string.Empty,
        0);

     // HCCGetAccountListResponseData responseData = Engine.Engine.ProcessRequest(request, 256) as HCCGetAccountListResponseData;

      var responseData = SessionCache.SessionCache.GetProcessRequest<HCCGetAccountListResponseData>(request, 256);

      //Assert.IsTrue(responseData.IsSuccess);
      //Assert.AreNotEqual("", responseData.ToXML());
      // Assert.IsTrue(responseData.ToXML().Contains("accounts"));
      Assert.IsTrue(responseData.Response != null);

      //Debug.WriteLineIf(responseData != null, responseData.ToXML());
      //Assert.IsTrue(responseData != null && responseData.HCCResponse != null && responseData.HCCResponse.AccountList.Count > 0);
    }
  }
}
