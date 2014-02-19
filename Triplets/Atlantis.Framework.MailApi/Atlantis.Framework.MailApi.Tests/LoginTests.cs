using System;
using Atlantis.Framework.MailApi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MailApi.Tests
{
  [TestClass]
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
  }
}
