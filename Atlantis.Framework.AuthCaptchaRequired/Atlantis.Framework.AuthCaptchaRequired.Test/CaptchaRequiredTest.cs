using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.AuthCaptchaRequired.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using System.Diagnostics;

namespace Atlantis.Framework.AuthCaptchaRequired.Test
{
  [TestClass]
  public class CaptchaRequiredTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CaptchaRequiredTest()
    {
      string shopperId = "867900";
      string ipLoopback = "127.0.0.1";
    //  string ipAddress = "172.23.45.65";

      var request = new AuthCaptchaRequiredRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, ipLoopback);
      var response = (AuthCaptchaRequiredResponseData)Engine.Engine.ProcessRequest(request, 508);

      Assert.IsTrue(response.IsSuccess);
      Assert.IsFalse(response.IsCaptchaRequired);
      Assert.IsFalse(string.IsNullOrEmpty(response.ToXML()));
      Debug.WriteLine(response.ToXML());
    }
  }
}
