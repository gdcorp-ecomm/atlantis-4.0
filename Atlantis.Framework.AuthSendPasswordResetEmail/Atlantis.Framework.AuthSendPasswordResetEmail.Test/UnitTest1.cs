using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthResetPassword.Interface;
using Atlantis.Framework.AuthSendPasswordResetEmail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuthSendPasswordResetEmail.Test
{
  [TestClass]
  public class UnitTest1
  {
    private string _shopperId = "128529"; //"870183"; //scy2
    private int _plid = 1;
    private string _localizationCode = "es";

    [TestMethod]
    public void SendResetEmail()
    {
      var request = new AuthSendPasswordResetEmailRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _plid);
      var response = Engine.Engine.ProcessRequest(request, 561) as AuthSendPasswordResetEmailResponseData;

      Assert.IsTrue(response.StatusCode == AuthResetPasswordStatusCodes.Success);
      Assert.IsTrue(response.ValidationCodes.Count == 0);
    }

    [TestMethod]
    public void SendResetEmailIntl()
    {
      var request = new AuthSendPasswordResetEmailRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, _plid, _localizationCode);
      var response = Engine.Engine.ProcessRequest(request, 561) as AuthSendPasswordResetEmailResponseData;

      Assert.IsTrue(response.StatusCode == AuthResetPasswordStatusCodes.Success);
      Assert.IsTrue(response.ValidationCodes.Count == 0);
    }

    [TestMethod]
    public void SendResetEmailNoShopper()
    {
      var request = new AuthSendPasswordResetEmailRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _plid);
      var response = Engine.Engine.ProcessRequest(request, 561) as AuthSendPasswordResetEmailResponseData;

      Assert.IsTrue(response.StatusCode == AuthResetPasswordStatusCodes.Error);
      Assert.IsTrue(response.ValidationCodes.Contains(AuthValidationCodes.ValidateShopperIdRequired));
    }
    
    [TestMethod]
    public void SendResetEmailNoPlId()
    {
      var request = new AuthSendPasswordResetEmailRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, 0);
      var response = Engine.Engine.ProcessRequest(request, 561) as AuthSendPasswordResetEmailResponseData;

      Assert.IsTrue(response.StatusCode == AuthResetPasswordStatusCodes.Error);
      Assert.IsTrue(response.ValidationCodes.Contains(AuthValidationCodes.ValidatePrivateLabelIdRequired));
    }
  }
}
