using Atlantis.Framework.ShopperPrefGet.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShopperPrefGet.Tests
{
  /// <summary>
  /// Summary description for ShopperPrefGetTests
  /// </summary>
  [TestClass]
  public class ShopperPrefGetTests
  {
    public ShopperPrefGetTests()
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

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ShopperPrefGetBasic()
    {
      ShopperPrefGetRequestData request =
        new ShopperPrefGetRequestData("860316", string.Empty, string.Empty, string.Empty, 0);
      ShopperPrefGetResponseData response = (ShopperPrefGetResponseData)Engine.Engine.ProcessRequest(request, 261);

      Assert.IsTrue(response.IsSuccess);
      Assert.AreEqual("860316", response.ShopperId);
      Assert.AreEqual("EUR", response.Preferences["gdshop_currencyType"]);
      Assert.IsTrue(response.Preferences.ContainsKey("countryFlag"));
      Assert.IsFalse(response.Preferences.ContainsKey("affiliate"));
      System.Diagnostics.Debug.WriteLine(response.ToXML());
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ShopperPrefGetShopperNotAvailable()
    {
      ShopperPrefGetRequestData request =
        new ShopperPrefGetRequestData("55555", string.Empty, string.Empty, string.Empty, 0);
      ShopperPrefGetResponseData response = (ShopperPrefGetResponseData)Engine.Engine.ProcessRequest(request, 261);

      Assert.IsTrue(response.IsSuccess);
      Assert.AreEqual(null, response.Preferences);
    }
  }
}

