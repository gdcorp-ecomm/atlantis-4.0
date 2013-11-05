using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Sso.Interface;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Sso.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Sso.Impl.dll")]
  public class GetKeyAndValidateSignatureTests
  {
    private string username = "syukna";
    private string shopperId = "867900";
    private string password = "Seth1seth";
    private int privateLabelId = 1;


    private SsoValidateShopperAndGetTokenRequestData _tokenRequest;
    private SsoValidateShopperAndGetTokenResponseData _tokenResponse;

    private Token _token;


    [TestInitialize]
    public void TestSetup()
    {
      _tokenRequest = new SsoValidateShopperAndGetTokenRequestData(username, password, privateLabelId);
      _tokenResponse = (SsoValidateShopperAndGetTokenResponseData) Engine.Engine.ProcessRequest(_tokenRequest, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);
      _token = _tokenResponse.Token;
    }

    [TestMethod]
    public void ValidSignatureWithToken()
    {
      Assert.IsTrue(_token.IsSignatureValid);
    }

    [TestMethod]
    public void ValidSignatureWithExplicitTokenRawData()
    {

      var tokenRequest = new SsoValidateShopperAndGetTokenRequestData(username, password, privateLabelId);
      var tokenResponse = (SsoValidateShopperAndGetTokenResponseData)Engine.Engine.ProcessRequest(tokenRequest, SsoEngineRequests.SsoValidateShopperAndGetTokenRequest);
      var token = tokenResponse.Token;

      var mytoken = new Token();
      mytoken.data = token.data;
      mytoken.PrivateLabelId = token.PrivateLabelId;

      Assert.IsTrue(mytoken.IsSignatureValid);
    }

    [TestMethod]
    public void GetKeyWithRawData()
    {
      //var tokenRequest = new SsoGetTokenRequestData(username, password, privateLabelId);
    //  var tokenResponse = (SsoGetTokenResponseData) DataCache.DataCache.GetProcessRequest(tokenRequest, SsoEngineRequests.SsoGetTokenRequest);

      var keyRequest = new SsoGetKeyRequestData(_token.data, _token.PrivateLabelId);
      var keyResponse = (SsoGetKeyResponseData) DataCache.DataCache.GetProcessRequest(keyRequest, SsoEngineRequests.SsoGetKeyRequest);
      Assert.IsTrue(KeyHasAllData(keyResponse.Key));
    }

    [TestMethod]
    public void InvalidSignature()
    {
      _token.RawTokenData.Signature = "asdfasdf";
      Assert.IsFalse(_token.IsSignatureValid);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void ExceptionMissingTokenHeaderKeyId()
    {
      var token = new Token();
      token = _tokenResponse.Token;
      token.Header.kid = string.Empty;

      var keyRequest = new SsoGetKeyRequestData(token);
      var keyResponse = (SsoGetKeyResponseData) DataCache.DataCache.GetProcessRequest(keyRequest, SsoEngineRequests.SsoGetKeyRequest);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void ExceptionMissingRawData()
    {
      var token = new Token();
      token.Header.kid = "23423";
      token.data = string.Empty;

      var keyRequest = new SsoGetKeyRequestData(token);
      var keyResponse = (SsoGetKeyResponseData) DataCache.DataCache.GetProcessRequest(keyRequest, SsoEngineRequests.SsoGetKeyRequest);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void ExceptionInvalidRequestType()
    {
      var token = new Token();

      var keyRequest = new SsoGetKeyRequestData(token);
      var keyResponse = (SsoGetKeyResponseData) DataCache.DataCache.GetProcessRequest(keyRequest, 0890);
    }

    private bool KeyHasAllData(Key key)
    {
      var list = new List<string>();
      list.Add(key.code);
      list.Add(key.id);
      list.Add(key.message);
      list.Add(key.type);
      list.Add(key.data.alg);
      list.Add(key.data.e);
      list.Add(key.data.n);
      list.Add(key.data.kid);
      list.Add(key.data.kty);

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
  }
}
