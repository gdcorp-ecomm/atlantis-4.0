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
      var request = new DotTypeClaimsRequestData(12345);
      var response = (DotTypeClaimsResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeClaimsBadRequest()
    {
      var request = new DotTypeClaimsRequestData(-1);
      var response = (DotTypeClaimsResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(false, response.IsSuccess);
    }
  }
}
