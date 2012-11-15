using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.OAuthGetAuthenticationCode.Interface;

namespace Atlantis.Framework.OAuthGetAuthenticationCode.Test
{
  [TestClass]
  public class UnitTest1
  {
    private string palmsId = "1";//"df51e10ac1b8404986471fe471615bde";
    private string billingNamespace = "domain";
    private string resourceId = "123456";
    private string commonName = "abc.com";
    private string accessList = "dns update";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.OAuthGetAuthenticationCode.Impl.dll")]
    public void GetOAuthCode()
    {
      var request = new OAuthGetAuthenticationCodeRequestData("867900", string.Empty, string.Empty, string.Empty, 0,
                                                              palmsId, billingNamespace, resourceId, commonName,
                                                              accessList);
      var response = Engine.Engine.ProcessRequest(request, 615) as OAuthGetAuthenticationCodeResponseData;
      string s = "pausse";

    }
  }
}
