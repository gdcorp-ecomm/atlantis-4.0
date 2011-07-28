using System.Threading;
using Atlantis.Framework.AuthChangePassword.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordNoCapital()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _pw1, "weakpassword", "weak pw", _userName1, true);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.StatusCodes.Contains(AuthChangePasswordStatusCodes.PasswordStrengthNoCapital));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordNoNumber()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _pw1, "Weakpassword", "weak pw", _userName1, true);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.StatusCodes.Contains(AuthChangePasswordStatusCodes.PasswordStrengthNoNumeric));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(AtlantisException))]
    public void ChangePasswordNonsecureWSInConfig()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _pw1, "weakpassword", "weak pw", _userName1, true);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 1071);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordMinLength()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _pw1, "Wkpw5", "weak pw", _userName1, true);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.StatusCodes.Contains(AuthChangePasswordStatusCodes.PasswordToShort));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordLoginNameUsed()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _pw1, _pw1, _hint1, "testarvind", false);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.StatusCodes.Contains(AuthChangePasswordStatusCodes.LoginAlreadyTaken));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordInvalidAuthentication()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, "password7", _pw1, _hint1, _userName1, false);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.StatusCodes.Contains(AuthChangePasswordStatusCodes.Failure));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordInvalidHintPassword()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _userName1, _pw1, _pw1, _userName1, false);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.StatusCodes.Contains(AuthChangePasswordStatusCodes.PasswordHintMatch));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordInvalidHintLogin()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _pw1, _pw1, _userName1, _userName1, false);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.StatusCodes.Contains(AuthChangePasswordStatusCodes.LoginHintMatch));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordInvalidLoginPassword()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _pw1, _userName1, _hint1, _userName1, false);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.StatusCodes.Contains(AuthChangePasswordStatusCodes.LoginPasswordMatch));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangePasswordValid()
    {
      AuthChangePasswordRequestData request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _pw1, _pw2, _hint1, _userName1, false);
      AuthChangePasswordResponseData response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsTrue(response.IsSuccess);

      Thread.Sleep(500);

      request = new AuthChangePasswordRequestData(
        _shopperId, string.Empty, string.Empty, string.Empty, 0,
        1, _pw2, _pw1, _hint1, _userName1, false);
      response = (AuthChangePasswordResponseData)Engine.Engine.ProcessRequest(request, 71);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
