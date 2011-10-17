using Atlantis.Framework.BillingResourceId.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.BillingResourceId.Tests
{
  /// <summary>
  /// </summary>
  [TestClass]
  public class BillingResourceIdTests
  {
    public BillingResourceIdTests()
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
    public void BillingResourceIdTest()
    {
      int productId = 119;
      string shopperId = "850659";
      BillingResourceIdRequestData request = new BillingResourceIdRequestData(
        shopperId,
        null,
        null,
        null,
        0,
        productId);

      BillingResourceIdResponseData response = (BillingResourceIdResponseData)Engine.Engine.ProcessRequest(request, 204);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BillingResourceIdFailTest()
    {
      int productId = 120;
      string shopperId = "832652";
      BillingResourceIdRequestData request = new BillingResourceIdRequestData(
        shopperId,
        null,
        null,
        null,
        0,
        productId);

      BillingResourceIdResponseData response = (BillingResourceIdResponseData)Engine.Engine.ProcessRequest(request, 204);

    }

  }
}
