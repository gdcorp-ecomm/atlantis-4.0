﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.AuthTwoFactorValidateToken.Interface;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorValidateToken.Test
{
  [TestClass]
  public class ValidateTokenTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void InvalidTokenTest()
    {
      string shopperId = "867900";
      string phoneNumber = "5555555555";
      string hostName = "host";
      string ipAddress = "172.23.45.65";
      string authToken = "2893X2";

      var request = new AuthTwoFactorValidateTokenRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, authToken, phoneNumber, hostName, ipAddress);
      var response = Engine.Engine.ProcessRequest(request, 510) as AuthTwoFactorValidateTokenResponseData;

      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Error);
      Assert.IsTrue(response.ValidationCodes.Count == 0);
      Assert.IsFalse(response.IsTokenValid);
      
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ValidTokenTest()
    {
      string shopperId = "867900";
      string phoneNumber = "15082415881";
      string hostName = "idp.godaddy.com";
      string ipAddress = "172.23.45.65";
      string authToken = "454621";

      var request = new AuthTwoFactorValidateTokenRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, authToken, phoneNumber, hostName, ipAddress);
      var response = Engine.Engine.ProcessRequest(request, 510) as AuthTwoFactorValidateTokenResponseData;

      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
      Assert.IsTrue(response.ValidationCodes.Count == 0);
      Assert.IsTrue(response.IsTokenValid);

    }
  }
}
