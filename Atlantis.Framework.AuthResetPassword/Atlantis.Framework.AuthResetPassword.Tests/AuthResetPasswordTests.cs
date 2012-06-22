using Atlantis.Framework.Auth.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.AuthResetPassword.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthResetPassword.Tests
{
  [TestClass]
  public class AuthResetPasswordTests
  {
    // shopper in DEV used by these tests (made specifically for them)
    private const string ShopperId = "856084";
    private const string Pw1 = "password";
    private const string Pw2 = "Wkpw";
    private const string Hint1 = "pw";
    private const string Hint2 = "weak pw";
    private const string IpAddress = "127.0.0.1";
    private const string HostName = "www.authresetpasswordunittests.com";

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    [ExpectedException( typeof( AtlantisException ) )]
    public void ResetPasswordNonsecureWsInConfig()
    {
      AuthResetPasswordRequestData request = new AuthResetPasswordRequestData(ShopperId, 
                                                                              string.Empty, 
                                                                              string.Empty, 
                                                                              string.Empty, 
                                                                              0,
                                                                              1, 
                                                                              IpAddress, 
                                                                              HostName, 
                                                                              Pw1, 
                                                                              Hint1, 
                                                                              string.Empty);

      Engine.Engine.ProcessRequest(request, 206);
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void ResetPasswordMinLength()
    {
      AuthResetPasswordRequestData request = new AuthResetPasswordRequestData(ShopperId, 
                                                                              string.Empty, 
                                                                              string.Empty, 
                                                                              string.Empty, 
                                                                              0,
                                                                              1, 
                                                                              IpAddress, 
                                                                              HostName, 
                                                                              Pw2, 
                                                                              Hint2, 
                                                                              string.Empty);

      AuthResetPasswordResponseData response = (AuthResetPasswordResponseData)Engine.Engine.ProcessRequest( request, 206 );
      Assert.IsFalse(response.StatusCode == AuthResetPasswordStatusCodes.Success);
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void ResetPasswordRequired()
    {
      AuthResetPasswordRequestData request = new AuthResetPasswordRequestData(string.Empty,
                                                                              string.Empty,
                                                                              string.Empty,
                                                                              string.Empty, 
                                                                              0,
                                                                              1,
                                                                              string.Empty,
                                                                              string.Empty,
                                                                              string.Empty,
                                                                              string.Empty,
                                                                              string.Empty);

      AuthResetPasswordResponseData response = (AuthResetPasswordResponseData)Engine.Engine.ProcessRequest( request, 206 );
      Assert.IsFalse(response.StatusCode == AuthResetPasswordStatusCodes.Success);
      Assert.IsTrue(response.ValidationCodes.Contains(AuthValidationCodes.ValidateShopperIdRequired));
      Assert.IsTrue(response.ValidationCodes.Contains(AuthValidationCodes.ValidateIpAddressRequired));
      Assert.IsTrue(response.ValidationCodes.Contains(AuthValidationCodes.ValidatePasswordRequired));
      Assert.IsTrue(response.ValidationCodes.Contains(AuthValidationCodes.ValidateHintRequired));
      Assert.IsTrue(response.ValidationCodes.Contains(AuthValidationCodes.ValidateAuthTokenRequired));
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void ResetPasswordInvalidHintPassword()
    {
      AuthResetPasswordRequestData request = new AuthResetPasswordRequestData(ShopperId,
                                                                              string.Empty,
                                                                              string.Empty,
                                                                              string.Empty, 
                                                                              0,
                                                                              1, 
                                                                              IpAddress,
                                                                              HostName, 
                                                                              Pw1, 
                                                                              Pw1, 
                                                                              string.Empty);

      AuthResetPasswordResponseData response = (AuthResetPasswordResponseData)Engine.Engine.ProcessRequest( request, 206 );
      Assert.IsFalse(response.StatusCode == AuthResetPasswordStatusCodes.Success);
      Assert.IsTrue(response.ValidationCodes.Contains(AuthValidationCodes.ValidatePasswordHintMatch));
    }
  }
}
