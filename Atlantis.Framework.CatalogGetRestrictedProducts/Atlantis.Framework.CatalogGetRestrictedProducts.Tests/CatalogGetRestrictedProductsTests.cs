using Atlantis.Framework.CatalogGetRestrictedProducts.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CatalogGetRestrictedProducts.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class CatalogGetRestrictedProductsTests
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
    public void GetRestrictedProducts1()
    {
      string mgrUserId = "945";
      
      CatalogGetRestrictedProductsRequestData requestData = new CatalogGetRestrictedProductsRequestData("859147",string.Empty,string.Empty,string.Empty,0,"1");
      CatalogGetRestrictedProductsResponseData response = (CatalogGetRestrictedProductsResponseData)DataCache.DataCache.GetProcessRequest(requestData, 243);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.RestrictedProductsByMgrUserId[mgrUserId].Contains(4603));
     
    }       
  }
}
