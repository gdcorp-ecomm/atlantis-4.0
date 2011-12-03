using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CatalogGetCategoriesCache.Interface;

namespace Atlantis.Framework.CatalogGetCategoriesCache.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class CatalogGetCategoriesCacheTests
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
      CatalogGetCategoriesCacheRequestData requestData = new CatalogGetCategoriesCacheRequestData("859147", string.Empty, string.Empty, string.Empty,0,"1",2);
      CatalogGetCategoriesCacheResponseData response = (CatalogGetCategoriesCacheResponseData)DataCache.DataCache.GetProcessRequest(requestData, 245);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.AdminOnlyProducts.Count > 0 && response.RestrictedProducts.Count > 0 && response.AllProducts.Count > 0);

    } 
  }
}
