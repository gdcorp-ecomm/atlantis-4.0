using System.Diagnostics;
using Atlantis.Framework.AuthTwoFactorDeletePending.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthTwoFactorDeletePending.Tests
{
  [TestClass]
  public class DeletePending
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DeletePendingTest()
    {
      AuthTwoFactorDeletePendingRequestData request = new AuthTwoFactorDeletePendingRequestData("850774", string.Empty, string.Empty, string.Empty, 0, 1, "Host", "127.0.0.1");
      AuthTwoFactorDeletePendingResponseData response = (AuthTwoFactorDeletePendingResponseData)Engine.Engine.ProcessRequest(request, 514);
      Debug.WriteLine("StatusCode: " + response.StatusCode);
      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
    }
  }
}
