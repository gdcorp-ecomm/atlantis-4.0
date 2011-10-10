using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCGetNameservers.Interface;

namespace Atlantis.Framework.DCCGetNameservers.Tests
{
  [TestClass]
  public class DCCGetNamserversTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetNameserversValid()
    {
      DCCGetNameserversRequestData request = new DCCGetNameserversRequestData("839627", string.Empty, string.Empty, string.Empty, 0, "0KAJSDHFJKAHSDF.COM", "MOBILE_CSA_DCC");
      DCCGetNameserversResponseData response = (DCCGetNameserversResponseData)Engine.Engine.ProcessRequest(request, 113);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetNameserversForDomainThatShopperDoesNotOwn()
    {
      DCCGetNameserversRequestData request = new DCCGetNameserversRequestData("847235", string.Empty, string.Empty, string.Empty, 0, "0KAJSDHFJKAHSDF.COM", "MOBILE_CSA_DCC");
      DCCGetNameserversResponseData response = (DCCGetNameserversResponseData)Engine.Engine.ProcessRequest(request, 113);
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
