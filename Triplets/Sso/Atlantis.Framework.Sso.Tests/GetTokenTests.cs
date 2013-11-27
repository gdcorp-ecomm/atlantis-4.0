using System;
using System.Collections.Generic;
using System.Runtime;
using System.Security.Cryptography;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Sso.Interface;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Sso.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Sso.Impl.dll")]
  public class GetTokenTests
  {
    private string username = "syukna";
    private string shopperId = "867900";
    private string password = "Seth1seth";
    private string clientIp = "172.23.44.142";
    private int privateLabelId = 1;

    private string usernameLocked = "syukna2";
    private string shopperIdLocked = "902172";

    private string usernameTwoFactor = "syuknatf";
    private string shopperIdTwoFactor = "870982";

    private SsoValidateShopperAndGetTokenRequestData tokenRequest;
    private SsoValidateShopperAndGetTokenResponseData tokenResponse;

    private SsoValidateShopperAndGetTokenRequestData tokenRequestInvalid;
    private SsoValidateShopperAndGetTokenResponseData tokenResponseInvalid;

    private SsoValidateShopperAndGetTokenRequestData tokenRequestLocked;
    private SsoValidateShopperAndGetTokenResponseData tokenResponseLocked;

    private SsoValidateShopperAndGetTokenRequestData tokenRequestTwoFactor;
    private SsoValidateShopperAndGetTokenResponseData tokenResponseTwoFactor;


    [TestInitialize]
    
    public void TestSetup()
    {
      tokenRequest = new SsoValidateShopperAndGetTokenRequestData(username, password,  privateLabelId, clientIp, "testUserAgent");
      tokenResponse = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(tokenRequest, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);

      tokenRequestInvalid = new SsoValidateShopperAndGetTokenRequestData("junk", password,  privateLabelId, clientIp);
      tokenResponseInvalid = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(tokenRequestInvalid, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);


      tokenRequestLocked = new SsoValidateShopperAndGetTokenRequestData(usernameLocked, password,  privateLabelId, clientIp);
      tokenResponseLocked = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(tokenRequestLocked, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);

      tokenRequestTwoFactor = new SsoValidateShopperAndGetTokenRequestData(usernameTwoFactor, password,  privateLabelId, clientIp);
      tokenResponseTwoFactor = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(tokenRequestTwoFactor, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);


    }

    #region Simple Data Exists Tests
    [TestMethod]
    public void GetTokenResponse()
    {
      Assert.IsNotNull(tokenResponse);
    }

    [TestMethod]
    public void GetTokenType()
    {
      Assert.IsTrue(tokenResponse.Token.type.Length > 0);
    }

    [TestMethod]
    public void GetTokenId()
    {
      Assert.IsTrue(tokenResponse.Token.id.Length > 0);
    }

    [TestMethod]
    public void GetTokenCode()
    {
      Assert.IsTrue(tokenResponse.Token.code.Length > 0);
    }

    [TestMethod]
    public void GetTokenMessage()
    {
      Assert.IsTrue(tokenResponse.Token.message.Length > 0);
    }

    [TestMethod]
    public void GetTokenData()
    {
      Assert.IsTrue(tokenResponse.Token.data.Length > 0);
    }
    
    [TestMethod]
    public void GetTokenExpires()
    {
      Assert.IsTrue(tokenResponse.Token.ExpiresAt > DateTime.Now);
    }
    
    [TestMethod]
    public void GetTokenIssuedAt()
    {
      Assert.IsTrue(tokenResponse.Token.IssuedAt > DateTime.Now.AddMinutes(-10));
    }
    #endregion

    #region Authentication Tests

    [TestMethod]
    public void ValidAccount()
    {
      Assert.IsTrue(tokenResponse.Token.code == SsoAuthApiResponseCodes.Success);
    }

    [TestMethod]
    public void InvalidAccount()
    {
      Assert.IsTrue(tokenResponseInvalid.Token.code == SsoAuthApiResponseCodes.FailureInvalidAttempt);
    }

    [TestMethod]
    public void LockedAccount()
    {
      Assert.IsTrue(tokenResponseLocked.Token.code == SsoAuthApiResponseCodes.FaliureAdminOrFraudLocked);
    }

    [TestMethod]
    public void ValidTwoFactor()
    {

      tokenRequestTwoFactor = new SsoValidateShopperAndGetTokenRequestData(usernameTwoFactor, password,  privateLabelId, clientIp);
      tokenResponseTwoFactor = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(tokenRequestTwoFactor, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);
      Assert.IsTrue(tokenResponseTwoFactor.Token.code == SsoAuthApiResponseCodes.SuccessTwoFactor);
    }

    [TestMethod]
    [Ignore]
    //NOTE THIS TEST NEEDS TO BE DONE MANUALLY DUE TO THE TWO FACTOR TOKEN BEING SENT TO A PHONE
    public void ValidateTwoFactorCode()
    {
      tokenRequestTwoFactor = new SsoValidateShopperAndGetTokenRequestData(usernameTwoFactor, password,  privateLabelId, clientIp);
      tokenResponseTwoFactor = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(tokenRequestTwoFactor, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);

      var tokenData = "eyJhbGciOiJkaXIiLCJraWQiOiJ6d0Qwbk5jYUxRIiwiZW5jIjoiQTEyOENCQy1IUzI1NiJ9..l7A1OOKWOwNo23In1kjo9w.RjGIFzhsyswE6hJNeHI-gv32516ziFVrHuRg4G3naCR8AB-20m96LtTOKqLb5jG-QWZRMBut3CdcBt7-F53URsr8rWU8ltupVQyZ9mE_NLflsfTrf8nI37t6CDD-NQFoEJi7EflBOHjz0x-IRX927YcgXnutsVbnx0BWfSSBYFWKPnuRoN9osXE4BBItQKdq56U7A42JBT9j4vaatdMe20wpTvIPYx_vL1kdHQW7s4iP1c9egj3e2uTWS1NbSyKy.dGJk_rbcmN85nxHnYXLPYQ";
      string putBreakPointHereWhenTestingAndItWillAllowYouToAddTheTwoFactorCode = "";

      var validateReq = new SsoValidateTwoFactorRequestData(tokenData, "276045");
      var validateRes = (SsoValidateTwoFactorResponseData) Engine.Engine.ProcessRequest(validateReq, SsoEngineRequests.SsoValidateTwoFactorRequest);

      Assert.IsTrue(PayloadHasAllData(validateRes.Token.Payload, true));
      Assert.IsTrue(HeaderHasAllData(validateRes.Token.Header));
      Assert.IsTrue(validateRes.Token.Signature.Length > 0);
      Assert.IsTrue(validateRes.Token.code == SsoAuthApiResponseCodes.Success);
      Assert.IsTrue(validateRes.Token.Payload.shopperId == shopperIdTwoFactor);

    }

    [TestMethod]
    public void TwoFactorBeforeValidateCode()
    {
      string tokenData = "eyJhbGciOiJkaXIiLCJraWQiOiJqNVF1LTdfdXpRIiwiZW5jIjoiQTEyOENCQy1IUzI1NiJ9..dDZIVYR2yu6x8poL4mwldg.whTi6fKDYCwmdTmw278-xfjsZ8xOwBGWIj4Y2-kRNH32UjojKYQzyzL4tovDtvf7HPeDM6j4V2GUr3m3wroyRaLDU9INgK4G2czvK_n9zHcDOmlnAJKRSujkKXlAT1dHjVfiv-0YTUrW1y9P0TsCaGmj-xjL2KuPzQXX7E9AD1LTBP8kghHyUU_rh_WhFkItADuHHO7WqG2baIn7Jl2RnWYzVr9kqdqOVazpsHlBJEMPMm95LZs8PJiAN3HklA4U.t0jm5IVEDLMfdpJIT6-zoQ";
      var token = new Token();
      token.data = tokenData;
      token.PrivateLabelId = 1;

      bool z = token.IsSignatureValid;
    }
    #endregion

    #region Exception Tests

    [TestMethod]
    [ExpectedException(typeof(Atlantis.Framework.Interface.AtlantisException))]
    public void ExceptionMissingPrivateLabel()
    {
      var request = new SsoValidateShopperAndGetTokenRequestData(string.Empty, string.Empty, 0, clientIp);
      var response = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(request, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);

    }

    [TestMethod]
    [ExpectedException(typeof(Atlantis.Framework.Interface.AtlantisException))]
    public void ExceptionMissingUsernameLabel()
    {
      var request = new SsoValidateShopperAndGetTokenRequestData(string.Empty, password,  privateLabelId, clientIp);
      var response = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(request, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);

    }

    [TestMethod]
    [ExpectedException(typeof(Atlantis.Framework.Interface.AtlantisException))]
    public void ExceptionMissingPasswordLabel()
    {
      var request = new SsoValidateShopperAndGetTokenRequestData(username, string.Empty,  privateLabelId, clientIp);
      var response = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(request, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);

    }

    [TestMethod]
    [ExpectedException(typeof(Atlantis.Framework.Interface.AtlantisException))]
    public void ExceptionInvalidRequestType()
    {
      var request = new SsoValidateShopperAndGetTokenRequestData(username, string.Empty,  privateLabelId, clientIp);
      var response = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(request, 1234);

    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void ExceptionTwoFactorMissingEncryptedData()
    {
      var request = new SsoValidateTwoFactorRequestData(string.Empty, "134");
      var response = (SsoValidateTwoFactorResponseData)Engine.Engine.ProcessRequest(request, SsoEngineRequests.SsoValidateTwoFactorRequest);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void ExceptionTwoFactorMissingToken()
    {
      var request = new SsoValidateTwoFactorRequestData("asdf", "");
      var response = (SsoValidateTwoFactorResponseData)Engine.Engine.ProcessRequest(request, SsoEngineRequests.SsoValidateTwoFactorRequest);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void ExceptionTwoFactorInvalidRequestType()
    {
      var request = new SsoValidateTwoFactorRequestData(string.Empty, "134");
      var response = (SsoValidateTwoFactorResponseData)Engine.Engine.ProcessRequest(request, 1234);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    [Ignore]
    public void ExceptionInvalidHttpResponseType()
    {
      try
      {
      var request = new SsoValidateShopperAndGetTokenRequestData("syukna", "Seth1seth", privateLabelId, clientIp);
      var response = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(request, 9999999);

      }
      catch (AtlantisException ex)
      {
        Assert.IsTrue(ex.Message.Contains("Unhandled http status code"));
        
        throw ex;
      }

    }
    #endregion

    #region Token Decrypted Data Tests
    
    [TestMethod]
    public void ValidInstancesHavePayloadData()
    {
      Assert.IsTrue(PayloadHasAllData(tokenResponse.Token.Payload));
      //Assert.IsTrue(PayloadHasAllData(tokenResponseInvalid.Token.Payload));
      //Assert.IsTrue(PayloadHasAllData(tokenResponseLocked.Token.Payload));
      //Assert.IsTrue(PayloadHasAllData(tokenResponseTwoFactor.Token.Payload, true));
    }

    [TestMethod]
    public void ValidInstancesHaveHeaderData()
    {
      Assert.IsTrue(HeaderHasAllData(tokenResponse.Token.Header));
    //  Assert.IsTrue(HeaderHasAllData(tokenResponseTwoFactor.Token.Header));
    }

    [TestMethod]
    public void ValidInstancesHaveSignature()
    {
      Assert.IsTrue(tokenResponse.Token.Signature.Length > 0);
   //   Assert.IsTrue(tokenResponseTwoFactor.Token.Signature.Length > 0);
    }

    private bool HeaderHasAllData(Header header)
    {
      var list = new List<string>();
      list.Add(header.alg);
      list.Add(header.kid);

      return AllFieldsHaveData(list);
    }

    private bool PayloadHasAllData(Payload payload, bool checkTwoFactor = false)
    {
      var list = new List<string>();
      list.Add(payload.exp);
      list.Add(payload.firstname);
      list.Add(payload.iat);
      list.Add(payload.jti);
      list.Add(payload.plid);
      list.Add(payload.shopperId);
      list.Add(payload.typ);
      list.Add(payload.factors.k_pw);

      if (checkTwoFactor)
      {
        list.Add(payload.factors.p_sms);
      }

      return AllFieldsHaveData(list);
    }

    private bool AllFieldsHaveData(List<string> values)
    {
      foreach (string value in values)
      {
        if (string.IsNullOrEmpty(value))
        {
          return false;
        }
      }

      return true;
    }
    #endregion
    
  }
}
