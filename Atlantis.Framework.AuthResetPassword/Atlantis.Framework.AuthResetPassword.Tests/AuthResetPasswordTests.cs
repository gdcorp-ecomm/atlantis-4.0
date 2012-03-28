﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.AuthResetPassword.Impl.AuthenticationWS;
using Atlantis.Framework.AuthResetPassword.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthResetPassword.Tests
{
  /// <summary>
  /// Summary description for AuthChangePasswordTests
  /// </summary>
  [TestClass]
  public class AuthResetPasswordTests
  {
    // shopper in DEV used by these tests (made specifically for them)
    private const string shopperId = "856084";
    private const string pw1 = "password";
    private const string pw2 = "Wkpw";
    private const string hint1 = "pw";
    private const string hint2 = "weak pw";
    private const string ipAddress = "0.0.0.0";

    private const string strongShopperId = "870937";
    private const string strongPw = "New1pass";

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
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
    [DeploymentItem( "atlantis.config" )]
    [ExpectedException( typeof( AtlantisException ) )]
    public void ResetPasswordNonsecureWsInConfig()
    {
      AuthResetPasswordRequestData request = new AuthResetPasswordRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0,
        1, ipAddress, pw1, hint1, "" );
      AuthResetPasswordResponseData response = (AuthResetPasswordResponseData)Engine.Engine.ProcessRequest( request, 1206 );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void ResetPasswordMinLength()
    {
      AuthResetPasswordRequestData request = new AuthResetPasswordRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0,
        1, ipAddress, pw2, hint2, "" );
      AuthResetPasswordResponseData response = (AuthResetPasswordResponseData)Engine.Engine.ProcessRequest( request, 206 );
      Assert.IsFalse( response.IsSuccess );
      Assert.IsTrue( response.StatusCodes.Contains( AuthResetPasswordStatusCodes.PasswordTooShort ) );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void ResetPasswordRequired()
    {
      AuthResetPasswordRequestData request = new AuthResetPasswordRequestData(
        String.Empty, String.Empty, String.Empty, String.Empty, 0,
        1, String.Empty, String.Empty, String.Empty, String.Empty );
      AuthResetPasswordResponseData response = (AuthResetPasswordResponseData)Engine.Engine.ProcessRequest( request, 206 );
      Assert.IsFalse( response.IsSuccess );
      Assert.IsTrue(response.StatusCodes.Contains(AuthResetPasswordStatusCodes.ValidateShopperIdRequired));
      Assert.IsTrue(response.StatusCodes.Contains(AuthResetPasswordStatusCodes.ValidateIpAddressRequired));
      Assert.IsTrue(response.StatusCodes.Contains(AuthResetPasswordStatusCodes.ValidatePasswordRequired));
      Assert.IsTrue(response.StatusCodes.Contains(AuthResetPasswordStatusCodes.ValidateHintRequired));
      Assert.IsTrue(response.StatusCodes.Contains(AuthResetPasswordStatusCodes.ValidateAuthTokenRequired));
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void ResetPasswordInvalidHintPassword()
    {
      AuthResetPasswordRequestData request = new AuthResetPasswordRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0,
        1, ipAddress, pw1, pw1, "" );
      AuthResetPasswordResponseData response = (AuthResetPasswordResponseData)Engine.Engine.ProcessRequest( request, 206 );
      Assert.IsFalse( response.IsSuccess );
      Assert.IsTrue( response.StatusCodes.Contains( AuthResetPasswordStatusCodes.PasswordHintMatch ) );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void ResetPasswordValid()
    {
      WScgdAuthenticateService authenticationService = new WScgdAuthenticateService();
      string authToken, error;
      int result = authenticationService.GetAuthToken( shopperId, 1, out authToken, out error );
      Assert.IsTrue( result == 1 );

      AuthResetPasswordRequestData request = new AuthResetPasswordRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0,
        1, ipAddress, pw1, hint1, authToken );
      AuthResetPasswordResponseData response = (AuthResetPasswordResponseData)Engine.Engine.ProcessRequest( request, 206 );
      Assert.IsTrue( response.IsSuccess );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void FailReset_PreviouslyUsedPassword()
    {
      var authenticationService = new WScgdAuthenticateService();
      string authToken, error;
      int result = authenticationService.GetAuthToken( shopperId, 1, out authToken, out error );
      Assert.IsTrue( result == 1 );

      var request = new AuthResetPasswordRequestData(strongShopperId, String.Empty, String.Empty, String.Empty, 0, 1, ipAddress, strongPw, hint1, authToken);
      var response = (AuthResetPasswordResponseData)Engine.Engine.ProcessRequest( request, 206 );
      Assert.IsTrue(response != null, "respone is null");
      Assert.IsFalse(response.IsSuccess, "response.IsSuccess is true when it should be false.");
      Assert.IsTrue(response.StatusCodes.Contains(AuthResetPasswordStatusCodes.PasswordPreviouslyUsed), string.Format("Missing status code returned. Should be: {0}. Found: {1}", AuthResetPasswordStatusCodes.PasswordPreviouslyUsed, string.Join(",", response.StatusCodes)));

    }
  }
}
