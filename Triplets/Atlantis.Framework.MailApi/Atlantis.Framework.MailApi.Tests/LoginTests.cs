using System;
using Atlantis.Framework.MailApi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MailApi.Tests
{
  [TestClass]
  public class LoginTests
  {
    [TestMethod]
    public void UsernameProperty()
    {
      var request = new LoginRequestData("username", "password");
      Assert.AreEqual("username", request.Username);
    }

    [TestMethod]
    public void PasswordProperty()
    {
      var request = new LoginRequestData("username", "password");
      Assert.AreEqual("password", request.Password);
    }
  }
}
