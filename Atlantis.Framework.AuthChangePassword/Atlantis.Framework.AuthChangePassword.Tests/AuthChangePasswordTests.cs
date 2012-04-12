using System.Threading;
using Atlantis.Framework.AuthChangePassword.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthChangePassword.Tests
{
  /// <summary>
  /// Summary description for AuthChangePasswordTests
  /// </summary>
  [TestClass]
  public class AuthChangePasswordTests
  {
    // shopper in DEV used by these tests (made specifically for them)
    private const string _shopperId = "856084";
    private const string _userName1 = "joepassword";
    private const string _userName2 = "joepassword2";
    private const string _pw1 = "password";
    private const string _pw2 = "password2";
    private const string _pw3 = "Password222";
    private const string _hint1 = "pw";
    private const string _hint2 = "pw2";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordCheck()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData("856151", string.Empty, string.Empty, string.Empty, 0, 1, "123456789", "JasonGerek101", _hint1, "srd101", string.Empty, string.Empty, "myhost", "127.0.0.1");
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
    }
  }
}
