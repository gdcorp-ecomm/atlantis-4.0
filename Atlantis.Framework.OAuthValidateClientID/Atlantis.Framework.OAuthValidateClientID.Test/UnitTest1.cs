using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.OAuthValidateClientId.Interface;

namespace Atlantis.Framework.OAuthValidateClientId.Test
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.OAuthValidateClientId.Impl.dll")]
  public class UnitTest1
  {
    //Dev App Id : 
    //251cc581dee54c4698fa0ce8b3f626c4
    //Test app Id: 
    //163f9fc158e24991865cbcff8369d8a7

    //Dev client id: 
    //n0huatakq83q3kz160v9niy266corahc

    //Test client id : 
    //hefm0owbty0ff3psb7ochyhvs0nx3b9l


    private string clientId = "n0huatakq83q3kz160v9niy266corahc";
    private string applicationId = "251cc581dee54c4698fa0ce8b3f626c4";
    private string ipAddress = "172.23.44.65";
    private string host = "idp";
    [TestMethod]
    public void ValidateClientId()
    {
      var request = new OAuthValidateClientIdRequestData("", string.Empty, string.Empty, string.Empty, 0, clientId,
                                                         applicationId, ipAddress , host);
      var response = Engine.Engine.ProcessRequest(request, 614) as OAuthValidateClientIdResponseData;

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.IsValidClientId);
      Assert.IsTrue(!string.IsNullOrEmpty(response.ClientPalmsId));
      Assert.IsTrue(string.IsNullOrEmpty(response.ErrorCode));
    }
  }
}
