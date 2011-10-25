using Atlantis.Framework.Engine;
using Atlantis.Framework.MyaRenewalBundleItems.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyaRenewalBundleItems.Tests
{
  [TestClass]
  public class MyaRenewalBundleItemsTests
  {
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

    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void MYAGetChildItems()
    {
      MyaRenewalBundleItemsRequestData requestData = new MyaRenewalBundleItemsRequestData(
        "822497", string.Empty, string.Empty, string.Empty, 0, 13689);      
      MyaRenewalBundleItemsResponseData response = (MyaRenewalBundleItemsResponseData)Engine.ProcessRequest(requestData, 196);
      var foo = response.BundleXML;
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
