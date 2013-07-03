using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.FlyoutMenuService.Interface;

namespace Atlantis.Framework.FlyoutMenuService.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.FlyoutMenuService.Impl.dll")]
  public class GetFlyoutMenuServiceTests
  {
    private const int REQUEST_TYPE = 715;

    public TestContext TestContext { get; set; }

    [TestMethod]
    public void FlyoutMenuItemServiceTest()
    {
      var request = new FlyoutMenuServiceRequestData(FlyoutMenuServiceRequestData.ServiceType.MenuItem);
      var response1 = DataCache.DataCache.GetProcessRequest(request, REQUEST_TYPE) as FlyoutMenuServiceResponseData;
      var response2 = DataCache.DataCache.GetProcessRequest(request, REQUEST_TYPE) as FlyoutMenuServiceResponseData;

      Assert.IsTrue(response1 != null);
      Assert.IsTrue(response2 != null);

      var xml1 = response1.ToXML();
      var xml2 = response2.ToXML();

      Assert.AreEqual(xml1, xml2);

      Assert.IsTrue(response1.MenuDataSet.Tables.Count > 0);
      Assert.IsTrue(response1.MenuDataSet.Tables.Count == response2.MenuDataSet.Tables.Count);
      Assert.IsTrue(response1.MenuDataSet.Tables[0].Rows.Count == response2.MenuDataSet.Tables[0].Rows.Count);
    }

    [TestMethod]
    public void FlyoutMenuSiteServiceTest()
    {
      var request = new FlyoutMenuServiceRequestData(FlyoutMenuServiceRequestData.ServiceType.MenuSite);
      var response1 = DataCache.DataCache.GetProcessRequest(request, REQUEST_TYPE) as FlyoutMenuServiceResponseData;
      var response2 = DataCache.DataCache.GetProcessRequest(request, REQUEST_TYPE) as FlyoutMenuServiceResponseData;

      Assert.IsTrue(response1 != null);
      Assert.IsTrue(response2 != null);

      var xml1 = response1.ToXML();
      var xml2 = response2.ToXML();

      Assert.AreEqual(xml1, xml2);
      Assert.IsTrue(response1.MenuDataSet.Tables.Count > 0);
      Assert.IsTrue(response1.MenuDataSet.Tables.Count == response2.MenuDataSet.Tables.Count);
      Assert.IsTrue(response1.MenuDataSet.Tables[0].Rows.Count == response2.MenuDataSet.Tables[0].Rows.Count);
    }
  }
}
