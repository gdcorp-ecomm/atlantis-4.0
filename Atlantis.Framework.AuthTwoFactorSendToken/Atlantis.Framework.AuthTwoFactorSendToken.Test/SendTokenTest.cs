using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.AuthTwoFactorSendToken.Interface;
using Atlantis.Framework.Auth.Interface;
using System.Diagnostics;

namespace Atlantis.Framework.AuthTwoFactorSendToken.Test
{
  [TestClass]
  public class SendTokenTest
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SendTokenFail()
    {
      string shopperId = "867900";
      string countryCode = "1";
      string phoneNumber = "1234567890";
      string hostName = "host";
      string ipAddress = "172.23.45.65";

      var request = new AuthTwoFactorSendTokenRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, countryCode, phoneNumber, hostName, ipAddress);
      var response = Engine.Engine.ProcessRequest(request, 509) as AuthTwoFactorSendTokenResponseData;

      Assert.IsFalse(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
      Debug.WriteLine(response.StatusMessage);
      Assert.IsTrue(response.ValidationCodes.Count == 0);
      Assert.IsFalse(response.AuthTokenSent);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SendTokenPass()
    {
      string shopperId = "847235";
      string countryCode = "1";
      string phoneNumber = "4807605267";
      string hostName = "idp.godaddy.com";
      string ipAddress = "172.23.44.65";

      var request = new AuthTwoFactorSendTokenRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, countryCode, phoneNumber, hostName, ipAddress);
      var response = Engine.Engine.ProcessRequest(request, 509) as AuthTwoFactorSendTokenResponseData;

      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
      Assert.IsTrue(response.ValidationCodes.Count == 0);
      Assert.IsTrue(response.AuthTokenSent);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SendTokenNoPhone()
    {
      string shopperId = "867900";
      string countryCode = "1";
      string phoneNumber = string.Empty;
      string hostName = "host";
      string ipAddress = "172.23.45.65";

      var request = new AuthTwoFactorSendTokenRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, countryCode, phoneNumber, hostName, ipAddress);
      var response = Engine.Engine.ProcessRequest(request, 509) as AuthTwoFactorSendTokenResponseData;

      Assert.IsFalse(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
      Assert.IsTrue(response.ValidationCodes.Contains(AuthValidationCodes.ValidatePhoneRequired));
      Assert.IsFalse(response.AuthTokenSent);
    }
  }
}
