using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ShopperPriceType.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using System;

namespace Atlantis.Framework.ShopperPriceType.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class ShopperPriceTypeTests
  {
    public ShopperPriceTypeTests()
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
    public void ShopperPriceTypeBasic()
    {
      ShopperPriceTypeRequestData request = new ShopperPriceTypeRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0, 1);
      ShopperPriceTypeResponseData response = (ShopperPriceTypeResponseData)Engine.Engine.ProcessRequest(request, 25);
      Assert.AreEqual(20, response.MaskedPriceType);
      Assert.AreEqual(16, response.ActivePriceType);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ShopperPriceTypeSession()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://mysite.com/default.aspx", string.Empty);

      ShopperPriceTypeRequestData request = new ShopperPriceTypeRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0, 1);

      ShopperPriceTypeResponseData response = SessionCache.SessionCache.GetProcessRequest<ShopperPriceTypeResponseData>(request, 25, TimeSpan.FromMinutes(5));
      Assert.AreEqual(4, response.MaskedPriceType);
      Assert.AreEqual(0, response.ActivePriceType);

      // Call again
      request = new ShopperPriceTypeRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0, 1);
      response = SessionCache.SessionCache.GetProcessRequest<ShopperPriceTypeResponseData>(request, 25, TimeSpan.FromMinutes(5));
      Assert.AreEqual(4, response.MaskedPriceType);
      Assert.AreEqual(0, response.ActivePriceType);
    }



  }
}
