using System;
using Atlantis.Framework.HDVDResetPassword.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDResetPassword.Tests
{
  [TestClass]
  public class HDVDResetPasswordTests
  {
    private const string _SHOPPER_ID = "858421";
    private const int _REQUEST_ID = 490;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordResetWithValidGuid()
    {
      // must be a VPH GUID - Reset Password does not work with DED
      const string accountGuid = "d11319d0-4d10-11e1-83a0-0050569575d8";
      const string newPassword = "NewPassword1";

      HDVDResetPasswordRequestData request = new HDVDResetPasswordRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        newPassword);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDResetPasswordResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDResetPasswordResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordResetWithInvalidGuid()
    {
      const string accountGuid = "bad";
      const string password = "Password3";

      HDVDResetPasswordRequestData request = new HDVDResetPasswordRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        password);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDResetPasswordResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDResetPasswordResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
