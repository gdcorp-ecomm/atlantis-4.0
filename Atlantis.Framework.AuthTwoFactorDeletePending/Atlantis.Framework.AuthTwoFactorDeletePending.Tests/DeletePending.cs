using System.Diagnostics;
using Atlantis.Framework.AuthTwoFactorDeletePending.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthTwoFactorDeletePending.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DeletePending
  {
    public DeletePending()
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
    public void DeletePendingTest()
    {

      AuthTwoFactorDeletePendingRequestData request = new AuthTwoFactorDeletePendingRequestData("850774", string.Empty, string.Empty, string.Empty, 0, 1, "Host", "127.0.0.1");
      AuthTwoFactorDeletePendingResponseData response = (AuthTwoFactorDeletePendingResponseData)Engine.Engine.ProcessRequest(request, 514);
      Debug.WriteLine("StatusCode: " + response.StatusCode);
      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);

    }
  }
}
