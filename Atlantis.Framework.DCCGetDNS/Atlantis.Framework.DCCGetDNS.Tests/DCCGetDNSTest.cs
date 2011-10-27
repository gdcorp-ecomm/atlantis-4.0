using Atlantis.Framework.DCCGetDNS.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCGetDNS.Tests
{
  [TestClass]
  public class DCCGetDNSTest
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetDNSValid()
    {
      DCCGetDNSRequestData request = new DCCGetDNSRequestData("9865", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");
      request.Type = "";
      DCCGetDNSResponseData response = (DCCGetDNSResponseData)Engine.Engine.ProcessRequest(request, 106);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetDNSForDomainThatShopperDoesNotOwn()
    {
      DCCGetDNSRequestData request = new DCCGetDNSRequestData("847235", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");
      request.Type = "";
      
      DCCGetDNSResponseData response = (DCCGetDNSResponseData)Engine.Engine.ProcessRequest(request, 106);

      Assert.IsTrue(!response.IsSuccess && !response.DnsZoneFileFound);
    }
  }
}
