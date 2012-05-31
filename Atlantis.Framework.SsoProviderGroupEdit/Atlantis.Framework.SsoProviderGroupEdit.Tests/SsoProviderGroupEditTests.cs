using Atlantis.Framework.SsoProviderGroupEdit.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SsoProviderGroupEdit.Tests
{
  [TestClass]
  public class SsoProviderGroupEditTests
  {
    private const int _requestType = 536;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void SsoProviderGroupEditTest()
    {
      string serviceProviderGroupName = "SSOEDITTEST";
      string redirectLoginUrl = "https://idp.dev.godaddy-com.ide/login.aspx";
      string logoutUrl = "https://idp.dev.godaddy-com.ide/logout.aspx";
      string redirectLogoutUrl = "https://idp.dev.godaddy-com.ide/logout.aspx";
      string changedBy = null;
      string approvedBy = null;
      string actionDescription = "TESTING - This is test data for the SSO Edit Tool";

      var request = new SsoProviderGroupEditRequestData(string.Empty
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , serviceProviderGroupName
        , redirectLoginUrl
        , logoutUrl
        , redirectLogoutUrl
        , changedBy
        , approvedBy
        , actionDescription);

      var response = (SsoProviderGroupEditResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
    }

  }
}
