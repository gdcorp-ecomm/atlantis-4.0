using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.AuthAuthorize.Interface;

namespace Atlantis.Framework.AuthAuthorize.Tests
{
  [TestClass]
  public class AuthAuthorizeTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthorizeSuccess()
    {
      AuthAuthorizeRequestData request = new AuthAuthorizeRequestData("832652", string.Empty, string.Empty, string.Empty, 0,
        "832652", "Password71", 1, "127.0.0.1");
      AuthAuthorizeResponseData response = (AuthAuthorizeResponseData)Engine.Engine.ProcessRequest(request, 396);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthorizeFail()
    {
      AuthAuthorizeRequestData request = new AuthAuthorizeRequestData("832652", string.Empty, string.Empty, string.Empty, 0,
        "832652", "nottherigtpassword", 1, "127.0.0.1");
      AuthAuthorizeResponseData response = (AuthAuthorizeResponseData)Engine.Engine.ProcessRequest(request, 396);
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
