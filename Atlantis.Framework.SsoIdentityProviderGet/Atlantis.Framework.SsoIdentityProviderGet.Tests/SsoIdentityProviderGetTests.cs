using System.Diagnostics;
using Atlantis.Framework.SsoIdentityProviderGet.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SsoIdentityProviderGet.Tests
{
  [TestClass]
  public class SsoIdentityProviderGetTests
  {
    private const string _identityProviderName = "BRIDP";
    private const int _requestType = 539;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void SsoIdentityProviderGetTest()
    {
      var request = new SsoIdentityProviderGetRequestData(string.Empty
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , _identityProviderName);

      var response = (SsoIdentityProviderGetResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      foreach (SsoIdentityProviderItem item in response.SsoIdentityProviders)
      {
        Debug.WriteLine(string.Format("IdentityProviderName: {0}", item.IdentityProviderName));
        Debug.WriteLine(string.Format("LoginUrl: {0}", item.LoginUrl));
        Debug.WriteLine(string.Format("LogoutUrl: {0}", item.LogoutUrl));
        Debug.WriteLine(string.Format("PublicKey: {0}", item.PublicKey));
        Debug.WriteLine(string.Format("CertificateName: {0}", item.CertificateName));
        Debug.WriteLine(string.Format("CreateDate: {0}", item.CreateDate));
        Debug.WriteLine(string.Format("ChangedBy: {0}", item.ChangedBy));
        Debug.WriteLine(string.Format("ApprovedBy: {0}", item.ApprovedBy));
        Debug.WriteLine(string.Format("ActionDescription: {0}", item.ActionDescription));
        Debug.WriteLine("******************************");
      }

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
