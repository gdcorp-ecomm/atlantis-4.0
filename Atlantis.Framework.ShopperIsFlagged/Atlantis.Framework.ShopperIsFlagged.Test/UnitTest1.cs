using System.Diagnostics;
using Atlantis.Framework.ShopperIsFlagged.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ShopperIsFlagged.Test
{
  [TestClass]
  public class UnitTest1
  {
    string shopperId = "867900"; //is flagged shopper 852910
     
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void GetIsShopperFlagged()
    {
      var request = new ShopperIsFlaggedRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0);
      var response = (ShopperIsFlaggedResponseData)Engine.Engine.ProcessRequest(request, 486);

      Debug.WriteLine("Shopper flagged: " + response.IsFlagged.ToString());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
