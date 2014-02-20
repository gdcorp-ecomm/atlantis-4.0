using System;
using System.Runtime.Versioning;
using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.MailApi.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MailApi.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.MailApi.Impl.dll")]
  [DeploymentItem("atlantis.config")]
  public class LoginTests
  {

    private const string ANDROID_APP_KEY = "tv2YfSzBx6zdjHAjIhW9mNe5";
    private const string IOS_APP_KEY = "PAiycbPel3SL1H6tj7UxNwUU";

    [TestMethod]
    public void UsernameProperty()
    {
      var request = new LoginRequestData("username", "password", ANDROID_APP_KEY);
      Assert.AreEqual("username", request.Username);
    }

    [TestMethod]
    public void PasswordProperty()
    {
      var request = new LoginRequestData("username", "password", ANDROID_APP_KEY);
      Assert.AreEqual("password", request.Password);
    }

    [TestMethod]
    public void AppKeyProperty()
    {
      var request = new LoginRequestData("username", "password", ANDROID_APP_KEY);
      Assert.AreEqual(ANDROID_APP_KEY, request.Appkey);
    }

    [TestMethod]
    public void GetLoginData()
    {

      var request = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);

      var response = (LoginResponseData)Engine.Engine.ProcessRequest(request, 798);
      Assert.IsNotNull(response);
    }

  }
}
