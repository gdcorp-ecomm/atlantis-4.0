using Atlantis.Framework.DotTypeClaims.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeClaims.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeClaims.Impl.dll")]
  public class DotTypeClaimsTests
  {
    [TestMethod]
    public void DotTypeClaimsGoodRequest()
    {
      string[] domains = {"domain1.shop", "domain2.shop"};

      var request = new DotTypeClaimsRequestData(domains);
      var response = (DotTypeClaimsResponseData)Engine.Engine.ProcessRequest(request, 710);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeClaimsBadRequest()
    {
      string[] noDomains = { "" };

      var request = new DotTypeClaimsRequestData(noDomains);
      var response = (DotTypeClaimsResponseData)Engine.Engine.ProcessRequest(request, 710);
      Assert.AreEqual(false, response.IsSuccess);
    }
  }
}
