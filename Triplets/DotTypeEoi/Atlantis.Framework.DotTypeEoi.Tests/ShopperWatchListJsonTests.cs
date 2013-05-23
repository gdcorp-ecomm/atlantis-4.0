using Atlantis.Framework.DotTypeEoi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeEoi.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeEoi.Impl.dll")]
  public class ShopperWatchListJsonTests
  {
    [TestMethod]
    public void ShopperWatchListJsonEmptyList()
    {
      var request = new ShopperWatchListJsonRequestData();
      request.ShopperID = "878145";
      var response = (ShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, 703);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, response.ShopperWatchListResponse != null);
      Assert.AreEqual(true, response.ShopperWatchListResponse.Gtlds.Count == 0);
    }
  }
}
