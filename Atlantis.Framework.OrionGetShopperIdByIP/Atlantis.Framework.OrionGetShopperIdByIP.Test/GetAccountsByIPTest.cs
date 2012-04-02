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
      var request = new OrionGetShopperIdByIPRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "127.0.0.1");
      var response = (OrionGetShopperIdByIPResponseData)Engine.Engine.ProcessRequest(request, 497);

      Assert.AreEqual("124917", response.ShopperId);
      Assert.IsTrue(response.IsSuccess);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void GetAccountByIp_IpNotFound()
    {
      /* NOTE - ALL CONFIG FILES ARE USING TEST SERVERS */
      var request = new OrionGetShopperIdByIPRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "000.000.000.000");
      var response = (OrionGetShopperIdByIPResponseData)Engine.Engine.ProcessRequest(request, 497);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.ShopperId == null);
    }


  }
}
