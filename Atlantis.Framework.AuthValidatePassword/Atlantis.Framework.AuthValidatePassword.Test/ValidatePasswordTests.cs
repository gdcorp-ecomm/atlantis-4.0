using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthValidatePassword.Interface;

namespace Atlantis.Framework.AuthValidatePassword.Test
{
  [TestClass]
  public class ValidatePasswordTests
  {
    public const string _shopperId = "867900";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordValid()
    {
      string password = "R34llyd00d?";
      var request = new AuthValidatePasswordRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, password);
      var response = Engine.Engine.ProcessRequest(request, 517) as AuthValidatePasswordResponseData;

      Assert.IsTrue(response.IsPasswordValid);
      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordBlacklist()
    {
      string password = "Password1234";
      var request = new AuthValidatePasswordRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, password);
      var response = Engine.Engine.ProcessRequest(request, 517) as AuthValidatePasswordResponseData;

      Assert.IsFalse(response.IsPasswordValid);
      Assert.IsTrue(response.StatusCode == AuthPasswordCodes.PasswordFailBlacklisted);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordMinLength()
    {
      string password = "Pxa4";
      var request = new AuthValidatePasswordRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, password);
      var response = Engine.Engine.ProcessRequest(request, 517) as AuthValidatePasswordResponseData;

      Assert.IsFalse(response.IsPasswordValid);
      Assert.IsTrue(response.StatusCode == AuthPasswordCodes.PasswordFailMinLength);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordMaxLength()
    {
      string password = "";
      for (int i = 0; i <= 256; i++)
      {
        password += "A";
      }

      var request = new AuthValidatePasswordRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, password);
      var response = Engine.Engine.ProcessRequest(request, 517) as AuthValidatePasswordResponseData;

      Assert.IsFalse(response.IsPasswordValid);
      Assert.IsTrue(response.StatusCode == AuthPasswordCodes.PasswordFailMaxLength);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordNoCapital()
    {
      string password = "r34llyd00d?";

      var request = new AuthValidatePasswordRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, password);
      var response = Engine.Engine.ProcessRequest(request, 517) as AuthValidatePasswordResponseData;

      Assert.IsFalse(response.IsPasswordValid);
      Assert.IsTrue(response.StatusCode == AuthPasswordCodes.PasswordFailNoCapital);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordNoNumber()
    {
      string password = "Reallydood?";

      var request = new AuthValidatePasswordRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, password);
      var response = Engine.Engine.ProcessRequest(request, 517) as AuthValidatePasswordResponseData;

      Assert.IsFalse(response.IsPasswordValid);
      Assert.IsTrue(response.StatusCode == AuthPasswordCodes.PasswordFailNoNumber);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordMatchesHint()  
    {
      string password = "this is my hint.  There are many like it, but this one is mine. 38820";

      var request = new AuthValidatePasswordRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0, password);
      var response = Engine.Engine.ProcessRequest(request, 517) as AuthValidatePasswordResponseData;

      Assert.IsFalse(response.IsPasswordValid);
      Assert.IsTrue(response.StatusCode == AuthPasswordCodes.PasswordFailMatchesHint);
    }
  }
}
