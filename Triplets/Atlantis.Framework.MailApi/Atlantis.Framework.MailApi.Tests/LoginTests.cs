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
    public void FromMockJsonDataValid()
    {

      var response = LoginResponseData.FromJsonData(Resources.ValidLoginData);

      Assert.IsNotNull(response.LoginData);
      Assert.IsNotNull(response.State);
      Assert.AreEqual("3ec646ddd52660180db869372bf86c36", response.LoginData.Hash);
      Assert.AreEqual("mailapi04.secureserver.net:443", response.LoginData.BaseUrl);
      Assert.AreEqual("https://mailapi04.secureserver.net/client.php?h=1a9605207194ceb558b40b1b9edc19ad", response.LoginData.ClientUrl);
      Assert.AreEqual("tv2YfSzBx6zdjHAjIhW9mNe5", response.State.AppKey);

    }

    [TestMethod]
    public void LoginDataFromWSValid()
    {

      var username = System.Web.HttpUtility.UrlEncode("tester@qa-emailpod04.com");  // Merc #159866
      var password = System.Web.HttpUtility.UrlEncode("Godaddy25");  // this is ALREADY escaped in GDAndroid code

      var request = new LoginRequestData(username, password, ANDROID_APP_KEY);

      var response = (LoginResponseData)Engine.Engine.ProcessRequest(request, 10350);
      Assert.IsNotNull(response);
    }

  }
}
