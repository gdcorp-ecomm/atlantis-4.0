using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCGetDomainContacts.Interface;

namespace Atlantis.Framework.DCCGetDomainContacts.Tests
{
  [TestClass]
  public class DCCGetDomainContactsTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetDomainContactValid()
    {
      DCCGetDomainContactsRequestData request = new DCCGetDomainContactsRequestData("839627", string.Empty, string.Empty, string.Empty, 0, "0KAJSDHFJKAHSDF.COM" , "MOBILE_CSA_DCC");
      DCCGetDomainContactsResponseData response = (DCCGetDomainContactsResponseData)Engine.Engine.ProcessRequest(request, 104);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCGetDomainContactForDomainThatShopperDoesNotOwn()
    {
      DCCGetDomainContactsRequestData request = new DCCGetDomainContactsRequestData("847235", string.Empty, string.Empty, string.Empty, 0, "0KAJSDHFJKAHSDF.COM", "MOBILE_CSA_DCC");
      DCCGetDomainContactsResponseData response = (DCCGetDomainContactsResponseData) Engine.Engine.ProcessRequest(request, 104);
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
