using Atlantis.Framework.DCCSetNameservers.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCSetNameservers.Tests
{
  [TestClass]
  public class DCCSetNameserversTests
  {
    [DeploymentItem("atlantis.config")]
    [TestMethod]
    public void TestSetNameservers()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("857020", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Forward, 1, 1666955, "MOBILE_CSA_DCC");
      request.AddCustomNameserver("ns1.foobar.net");
      request.AddCustomNameserver("ns2.foobar.net");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Assert.IsTrue(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [TestMethod]
    public void TestSetNameserversEmpty()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("857020", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Forward, 1, 1666955, "MOBILE_CSA_DCC");
      request.AddCustomNameserver(string.Empty);
      request.AddCustomNameserver(" ");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);
      Assert.IsFalse(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [TestMethod]
    public void TestSetNameserversForShopperThatDoesNotOwnDomain()
    {
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("847235", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Forward, 1, 1666955, "MOBILE_CSA_DCC");
      request.AddCustomNameserver("ns1.foobar.net");
      request.AddCustomNameserver("ns2.foobar.net");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);

      // Pending updates by the DCC team.  They are returning success in this case.
      Assert.IsFalse(response.IsSuccess);
    }

    [DeploymentItem("atlantis.config")]
    [TestMethod]
    public void SetNameserversForPremiumDnsShopper()
    {
      // TEST Shopper, APPLY-MY-PREMIUM-NS-ANDROID.ORG
      DCCSetNameserversRequestData request = new DCCSetNameserversRequestData("87409", string.Empty, string.Empty, string.Empty, 0, DCCSetNameserversRequestData.NameserverType.Park, 1, 1710527, "MOBILE_CSA_DCC");
      request.AddPremiumNameserver("PDNS01.TEST.DOMAINCONTROL.COM");
      request.AddPremiumNameserver("PDNS02.TEST.DOMAINCONTROL.COM");

      DCCSetNameserversResponseData response = (DCCSetNameserversResponseData)Engine.Engine.ProcessRequest(request, 114);

      // Pending updates by the DCC team.  They are returning success in this case.
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
