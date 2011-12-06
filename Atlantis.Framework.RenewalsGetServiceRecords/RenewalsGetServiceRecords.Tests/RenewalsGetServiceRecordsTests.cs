using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.RenewalsGetServiceRecords.Interface;
using Atlantis.Framework.Engine;

namespace RenewalsGetServiceRecords.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class RenewalsGetServiceRecordsTests
  {
    public RenewalsGetServiceRecordsTests()
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
    public void RenewalsGetHostedSites()
    {
      RenewalsGetServiceRecordsRequestData requestData = new RenewalsGetServiceRecordsRequestData(
        "822497", string.Empty, string.Empty, string.Empty, 0);//850774
      RenewalsGetServiceRecordsResponseData response = (RenewalsGetServiceRecordsResponseData)Engine.ProcessRequest(requestData, 177);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
