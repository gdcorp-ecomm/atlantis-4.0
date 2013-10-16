using Atlantis.Framework.Interface;
using Atlantis.Framework.Sso.Impl.Helpers;
using Atlantis.Framework.Sso.Interface;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;
using System;
using System.Text;

namespace Atlantis.Framework.Sso.Impl
{
  public class SsoValidateShopperAndGetTokenRequest : IRequest
  {
    private SsoValidateShopperAndGetTokenRequestData _ssoTokenRequestData;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoValidateShopperAndGetTokenResponseData response;

      try
      {
        _ssoTokenRequestData = (SsoValidateShopperAndGetTokenRequestData)requestData;
        ThrowExceptionIfRequestDataIsMissing();

        var byteData = GetTokenRequestByteData();
        var wsUrl = ((WsConfigElement)config).WSURL;

        var tokenWebRequest = HttpHelpers.GetHttpWebRequestAndAddByteData(wsUrl, byteData, "application/x-www-form-urlencoded", "POST");
        Token token = HttpHelpers.GetWebResponseAndConvertToObject<Token>(tokenWebRequest);
        token.PrivateLabelId = _ssoTokenRequestData.PrivateLabelId;

        response = new SsoValidateShopperAndGetTokenResponseData(token);
      }
      catch (AtlantisException aex)
      
      {
        response = new SsoValidateShopperAndGetTokenResponseData(aex);
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(requestData, "SSOGetTokenRequest::RequestHandler", ex.Message, "requestType: " + config.RequestType);
        response = new SsoValidateShopperAndGetTokenResponseData(aex);
      }

      return response;
    }

    private byte[] GetTokenRequestByteData()
    {
      var inputDataString = new StringBuilder(100);

      inputDataString.Append("username=");
      inputDataString.Append(_ssoTokenRequestData.Username);
      inputDataString.Append("&password=");
      inputDataString.Append(_ssoTokenRequestData.Password);

      if (_ssoTokenRequestData.PrivateLabelId != PrivateLabelIds.GoDaddy)
      {
        inputDataString.Append("&plid=");
        inputDataString.Append(_ssoTokenRequestData.PrivateLabelId);
      }

      var byteData = Encoding.UTF8.GetBytes(inputDataString.ToString());
      return byteData;
    }

    private void ThrowExceptionIfRequestDataIsMissing()
    {
      var isValid = !string.IsNullOrEmpty(_ssoTokenRequestData.Username)
                && !string.IsNullOrEmpty(_ssoTokenRequestData.Password)
                && _ssoTokenRequestData.PrivateLabelId > 0;

      if (!isValid)
      {
        var data = string.Concat("Username: ", _ssoTokenRequestData.Username,
            " | PasswordLength: ", _ssoTokenRequestData.Password.Length,
            " | PrivateLabelID: ", _ssoTokenRequestData.PrivateLabelId);

        throw new MissingFieldException("MissingRequestData: " + data);
      }
    }
  }
}
