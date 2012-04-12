using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.AuthTwoFactorStatus.Interface;

namespace Atlantis.Framework.AuthTwoFactorStatus.Tests
{
  [TestClass]
  public class TwoFactorStatus
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetStatus()
    {
      AuthTwoFactorStatusRequestData requestData = new AuthTwoFactorStatusRequestData("850774", string.Empty, string.Empty, string.Empty, 0);

      AuthTwoFactorStatusResponseData response = (AuthTwoFactorStatusResponseData)Engine.Engine.ProcessRequest(requestData, 515);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
