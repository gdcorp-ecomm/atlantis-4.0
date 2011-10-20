using Atlantis.Framework.ShopperCreditLine.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShopperCreditLine.Tests
{
  [TestClass]
  public class ShopperCreditLineTests
  {
    public ShopperCreditLineTests()
    {
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
    public void ShopperCreditLineBasicTest()
    {
      ShopperCreditLineRequestData request =
        new ShopperCreditLineRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      ShopperCreditLineResponseData response =
        (ShopperCreditLineResponseData)DataCache.DataCache.GetProcessRequest(request, 73);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
