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
    public void DCCSetAutoRenewValid()
    {
      DCCSetAutoRenewRequestData request = new DCCSetAutoRenewRequestData("857020", 1, 1666955, 1, "MOBILE_CSA_DCC", false, "0");
      DCCSetAutoRenewResponseData response = (DCCSetAutoRenewResponseData)Engine.Engine.ProcessRequest(request, 101);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DCCSetAutoRenew.Impl.dll")]
    public void DCCSetAutoRenewForDomainThatShopperDoesNotOwn()
    {
      DCCSetAutoRenewRequestData request = new DCCSetAutoRenewRequestData("840820", 1, 1666955, 1, "MOBILE_CSA_DCC", false, "0");
      DCCSetAutoRenewResponseData response = (DCCSetAutoRenewResponseData)Engine.Engine.ProcessRequest(request, 101);
      // Success is coming back true, DCC Team will be fixing this
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
