using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCSetAutoRenew.Interface;

namespace Atlantis.Framework.DCCSetAutoRenew.Tests
{
  [TestClass]
  public class DCCSetAutoRenewTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DCCSetAutoRenew.Impl.dll")]
    public void DccSetAutoRenewValid()
    {
      var request = new DCCSetAutoRenewRequestData("857020", 1, 1665465, 1, "MOBILE_CSA_DCC", false, "0");
      var response = (DCCSetAutoRenewResponseData)Engine.Engine.ProcessRequest(request, 101);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DCCSetAutoRenew.Impl.dll")]
    public void DccSetAutoRenewForDomainThatShopperDoesNotOwn()
    {
      var request = new DCCSetAutoRenewRequestData("840820", 1, 1666955, 1, "MOBILE_CSA_DCC", false, "0");
      var response = (DCCSetAutoRenewResponseData)Engine.Engine.ProcessRequest(request, 101);
      // Success is coming back true, DCC Team will be fixing this
      Assert.IsFalse(response.IsSuccess);
    }
  }
}