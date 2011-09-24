using System.Diagnostics;
using Atlantis.Framework.HCCSetupWST.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HCCSetupWST.Test
{
  /// <summary>
  /// Summary description for HCCSetupWSTTest
  /// </summary>
  [TestClass]
  public class HCCSetupWSTTest
  {
    public HCCSetupWSTTest()
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
    public void SetUpHostingAcct()
    {
      HCCSetupWSTRequestData request = new HCCSetupWSTRequestData("",
        "hccapitest",
        "Password1",
        "",
        string.Empty, // cert
        false, // enableGoogleWMT
        "12530",
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      var responseData = Engine.Engine.ProcessRequest(request, 327) as HCCSetupWSTResponseData;

      // Debug.Assert(responseData.Response.Errors.Length == 0, responseData.ToXML());
      Assert.IsTrue(responseData.Response.Errors.Length == 0, responseData.ToXML());
      Debug.WriteLine(responseData.ToXML());
      /******************************* A failed Test ***************************************
       Assert.IsTrue(responseData.Response.Errors.Length == 0, responseData.ToXML());
        Assert.IsTrue failed. <response>
        <message>Invalid Username.</message>
        <status>Failed</status>
        <statuscode>8888</statuscode>
        <errors>
          <error>Your user name can only contain letters and numbers.</error>
          <error>Your user name can only contain lowercase letters.</error>
        </errors>
      </response>
      ******************************* A failed Test ***************************************/
    }
  }
}
