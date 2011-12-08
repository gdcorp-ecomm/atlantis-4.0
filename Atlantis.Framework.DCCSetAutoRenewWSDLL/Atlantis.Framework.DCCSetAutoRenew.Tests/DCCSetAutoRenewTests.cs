using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCSetAutoRenew.Interface;

namespace Atlantis.Framework.DCCSetAutoRenew.Tests
{
  [TestClass]
  public class DCCSetAutoRenewTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCSetAutoRenewValid()
    {
      DCCSetAutoRenewRequestData request = new DCCSetAutoRenewRequestData("857020", string.Empty, string.Empty, string.Empty, 0, 1, 1666955, 1, "MOBILE_CSA_DCC");
      DCCSetAutoRenewResponseData response = (DCCSetAutoRenewResponseData)Engine.Engine.ProcessRequest(request, 101);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCSetAutoRenewForDomainThatShopperDoesNotOwn()
    {
      DCCSetAutoRenewRequestData request = new DCCSetAutoRenewRequestData("847235", string.Empty, string.Empty, string.Empty, 0, 1, 1666955, 1, "MOBILE_CSA_DCC");
      DCCSetAutoRenewResponseData response = (DCCSetAutoRenewResponseData)Engine.Engine.ProcessRequest(request, 101);
      // Success is coming back true, DCC Team will be fixing this
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
