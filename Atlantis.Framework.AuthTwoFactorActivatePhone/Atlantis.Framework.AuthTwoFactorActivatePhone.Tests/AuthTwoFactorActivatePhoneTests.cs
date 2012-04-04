using System;
using Atlantis.Framework.Auth.Interface;
using Atlantis.Framework.AuthTwoFactorActivatePhone.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuthTwoFactorActivatePhone.Tests
{
  [TestClass]
  public class AuthTwoFactorActivatePhoneTests
  {
    private const string VALID_SHOPPER_ID = "847235";
    private const string VALID_COUNTRY_CODE = "1";
    private const string VALID_PHONE = "4807605267";
    private const string VALID_AUTH_TOKEN = "2893X2"; // techincally this is not valid, for these unit test, we can't spin up a new one each time
    private const string VALID_IP = "172.23.45.65";
    private const string VALID_HOST = "www.AuthTwoFactorActivatePhoneTests.com";
    

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthTwoFactorActivatePhoneValid()
    {
      AuthTwoFactorActivatePhoneRequestData requestData = new AuthTwoFactorActivatePhoneRequestData(VALID_SHOPPER_ID,
                                                                                                    VALID_COUNTRY_CODE,
                                                                                                    VALID_PHONE,
                                                                                                    VALID_AUTH_TOKEN,
                                                                                                    VALID_IP,
                                                                                                    VALID_HOST,
                                                                                                     "www.AuthTwoFactorActivatePhoneTests.com/tests/",
                                                                                                    string.Empty,
                                                                                                    Guid.NewGuid().ToString(),
                                                                                                    1);

      AuthTwoFactorActivatePhoneResponseData responseData = (AuthTwoFactorActivatePhoneResponseData)Engine.Engine.ProcessRequest(requestData, 519);

      Assert.IsTrue(responseData.ValidationCodes.Count == 0);
      Assert.IsTrue(responseData.StatusCode == TwoFactorWebserviceResponseCodes.Success, string.Format("Status Code: {0}, Status Message: {1}", responseData.StatusCode, responseData.StatusMessage));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthTwoFactorActivatePhoneInValidShopper()
    {
      AuthTwoFactorActivatePhoneRequestData requestData = new AuthTwoFactorActivatePhoneRequestData(string.Empty,
                                                                                                    VALID_COUNTRY_CODE,
                                                                                                    VALID_PHONE,
                                                                                                    VALID_AUTH_TOKEN,
                                                                                                    VALID_IP,
                                                                                                    VALID_HOST,
                                                                                                     "www.AuthTwoFactorActivatePhoneTests.com/tests/",
                                                                                                    string.Empty,
                                                                                                    Guid.NewGuid().ToString(),
                                                                                                    1);

      AuthTwoFactorActivatePhoneResponseData responseData = (AuthTwoFactorActivatePhoneResponseData)Engine.Engine.ProcessRequest(requestData, 519);

      Assert.IsTrue(responseData.StatusCode == TwoFactorWebserviceResponseCodes.Error, string.Format("Status Code: {0}, Status Message: {1}", responseData.StatusCode, responseData.StatusMessage));
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidateShopperIdRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthTwoFactorActivatePhoneInValidPhone()
    {
      AuthTwoFactorActivatePhoneRequestData requestData = new AuthTwoFactorActivatePhoneRequestData(VALID_SHOPPER_ID,
                                                                                                    VALID_COUNTRY_CODE,
                                                                                                    string.Empty,
                                                                                                    VALID_AUTH_TOKEN,
                                                                                                    VALID_IP,
                                                                                                    VALID_HOST,
                                                                                                     "www.AuthTwoFactorActivatePhoneTests.com/tests/",
                                                                                                    string.Empty,
                                                                                                    Guid.NewGuid().ToString(),
                                                                                                    1);

      AuthTwoFactorActivatePhoneResponseData responseData = (AuthTwoFactorActivatePhoneResponseData)Engine.Engine.ProcessRequest(requestData, 519);

      Assert.IsTrue(responseData.StatusCode == TwoFactorWebserviceResponseCodes.Error, string.Format("Status Code: {0}, Status Message: {1}", responseData.StatusCode, responseData.StatusMessage));
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidatePhoneRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthTwoFactorActivatePhoneInValidAuthToken()
    {
      AuthTwoFactorActivatePhoneRequestData requestData = new AuthTwoFactorActivatePhoneRequestData(VALID_SHOPPER_ID,
                                                                                                    VALID_COUNTRY_CODE,
                                                                                                    VALID_PHONE,
                                                                                                    string.Empty,
                                                                                                    VALID_IP,
                                                                                                    VALID_HOST,
                                                                                                     "www.AuthTwoFactorActivatePhoneTests.com/tests/",
                                                                                                    string.Empty,
                                                                                                    Guid.NewGuid().ToString(),
                                                                                                    1);

      AuthTwoFactorActivatePhoneResponseData responseData = (AuthTwoFactorActivatePhoneResponseData)Engine.Engine.ProcessRequest(requestData, 519);

      Assert.IsTrue(responseData.StatusCode == TwoFactorWebserviceResponseCodes.Error, string.Format("Status Code: {0}, Status Message: {1}", responseData.StatusCode, responseData.StatusMessage));
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidateAuthTokenRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthTwoFactorActivatePhoneInValidIpAddress()
    {
      AuthTwoFactorActivatePhoneRequestData requestData = new AuthTwoFactorActivatePhoneRequestData(VALID_SHOPPER_ID,
                                                                                                    VALID_COUNTRY_CODE,
                                                                                                    VALID_PHONE,
                                                                                                    VALID_PHONE,
                                                                                                    string.Empty,
                                                                                                    VALID_HOST,
                                                                                                     "www.AuthTwoFactorActivatePhoneTests.com/tests/",
                                                                                                    string.Empty,
                                                                                                    Guid.NewGuid().ToString(),
                                                                                                    1);

      AuthTwoFactorActivatePhoneResponseData responseData = (AuthTwoFactorActivatePhoneResponseData)Engine.Engine.ProcessRequest(requestData, 519);

      Assert.IsTrue(responseData.StatusCode == TwoFactorWebserviceResponseCodes.Error, string.Format("Status Code: {0}, Status Message: {1}", responseData.StatusCode, responseData.StatusMessage));
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidateIpAddressRequired));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthTwoFactorActivatePhoneInValidHost()
    {
      AuthTwoFactorActivatePhoneRequestData requestData = new AuthTwoFactorActivatePhoneRequestData(VALID_SHOPPER_ID,
                                                                                                    VALID_COUNTRY_CODE,
                                                                                                    VALID_PHONE,
                                                                                                    VALID_PHONE,
                                                                                                    VALID_IP,
                                                                                                    string.Empty,
                                                                                                    "www.AuthTwoFactorActivatePhoneTests.com/tests/",
                                                                                                    string.Empty,
                                                                                                    Guid.NewGuid().ToString(),
                                                                                                    1);

      AuthTwoFactorActivatePhoneResponseData responseData = (AuthTwoFactorActivatePhoneResponseData)Engine.Engine.ProcessRequest(requestData, 519);

      Assert.IsTrue(responseData.StatusCode == TwoFactorWebserviceResponseCodes.Error, string.Format("Status Code: {0}, Status Message: {1}", responseData.StatusCode, responseData.StatusMessage));
      Assert.IsTrue(responseData.ValidationCodes.Contains(AuthValidationCodes.ValidateHostNameRequired));
    }
  }
}
