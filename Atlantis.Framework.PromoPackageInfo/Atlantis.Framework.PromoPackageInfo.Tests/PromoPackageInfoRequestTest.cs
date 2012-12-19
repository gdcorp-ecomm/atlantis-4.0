using Atlantis.Framework.PromoPackageInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PromoPackageInfo.Tests
{
  [TestClass()]
  public class PromoPackageInfoRequestTest
  {
    private TestContext testContextInstance;

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

    [TestMethod()]
    [DeploymentItem("Atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void DataCacheRequestTest()
    {
      PromoPackageInfoRequestData request = new PromoPackageInfoRequestData("858346", string.Empty, string.Empty, string.Empty, 0, 1);
      PromoPackageInfoResponseData response = (PromoPackageInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 632);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsNotNull(response.ProductList);
      Assert.IsTrue(0 < response.ProductList.Count);

      response = (PromoPackageInfoResponseData)DataCache.DataCache.GetProcessRequest(request, 632);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsNotNull(response.ProductList);
      Assert.IsTrue(0 < response.ProductList.Count);
    }

    /// <summary>
    ///A test for RequestHandler
    ///</summary>
    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.PromoPackageInfo.Impl.dll")]
    public void RequestHandlerTest()
    {
      PromoPackageInfoRequestData request = new PromoPackageInfoRequestData("858346", string.Empty, string.Empty, string.Empty, 0, 1);
      PromoPackageInfoResponseData actual = (PromoPackageInfoResponseData)Engine.Engine.ProcessRequest(request, 632);
      Assert.IsTrue(actual.IsSuccess);
      Assert.IsNotNull(actual.ProductList);
      Assert.IsTrue(0 < actual.ProductList.Count);
      PackageItem testobject = actual.ProductList[0];
      Assert.IsTrue(testobject.PackageGroupID > 0);
    }
  }
}
