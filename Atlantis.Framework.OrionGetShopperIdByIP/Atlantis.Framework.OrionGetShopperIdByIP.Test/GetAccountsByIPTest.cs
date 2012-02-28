using Atlantis.Framework.OrionGetShopperIdByIP.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.OrionGetShopperIdByIP.Test
{
  /**************************************************/
  /* NOTE - ALL CONFIG FILES ARE USING TEST SERVERS */
  /**************************************************/

  [TestClass]
  public class GetAccountsByIPTest
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void GetAccountByIp()
    {
      /* NOTE - ALL CONFIG FILES ARE USING TEST SERVERS */
      var request = new OrionGetShopperIdByIPRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "172.19.92.115");
      var response = (OrionGetShopperIdByIPResponseData)Engine.Engine.ProcessRequest(request, 497);

      Assert.AreEqual("124917", response.ShopperId);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
