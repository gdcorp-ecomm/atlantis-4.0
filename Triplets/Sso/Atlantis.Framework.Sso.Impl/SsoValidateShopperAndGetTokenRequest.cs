using System.Security.Cryptography.X509Certificates;
using System.Web;
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

        var wsConfigElement = ((WsConfigElement) config);

        var urlEncodedData = GetTokenRequestData();
        var wsUrl = wsConfigElement.WSURL;
        X509Certificate2 clientCert = wsConfigElement.GetClientCertificate();

        var tokenWebRequest = HttpHelpers.GetHttpWebRequestAndAddData(wsUrl, urlEncodedData, "application/x-www-form-urlencoded", "POST", _ssoTokenRequestData.RequestTimeout, clientCert);
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

    private string GetTokenRequestData()
    {
      var inputDataString = new StringBuilder(100);

      inputDataString.Append("username=");
      inputDataString.Append(HttpUtility.UrlEncode(_ssoTokenRequestData.Username));
      inputDataString.Append("&password=");
      inputDataString.Append(HttpUtility.UrlEncode(_ssoTokenRequestData.Password));
      inputDataString.Append("&user_ip=");
      inputDataString.Append(HttpUtility.UrlEncode(_ssoTokenRequestData.ClientIp));

      if (_ssoTokenRequestData.PrivateLabelId != PrivateLabelIds.GoDaddy && _ssoTokenRequestData.PrivateLabelId != PrivateLabelIds.DomainsByProxy)
      {
        inputDataString.Append("&plid=");
        inputDataString.Append(_ssoTokenRequestData.PrivateLabelId);
      }

      return inputDataString.ToString();
    }

    private void ThrowExceptionIfRequestDataIsMissing()
    {
      var isValid = !string.IsNullOrEmpty(_ssoTokenRequestData.Username)
                && !string.IsNullOrEmpty(_ssoTokenRequestData.Password)
                && !string.IsNullOrEmpty(_ssoTokenRequestData.ClientIp)
                && _ssoTokenRequestData.PrivateLabelId > 0;

      if (!isValid)
      {
        var data = string.Concat("Username: ", _ssoTokenRequestData.Username,
          " | PasswordLength: ", _ssoTokenRequestData.Password.Length,
          " | PrivateLabelID: ", _ssoTokenRequestData.PrivateLabelId,
          " | ClientIp: ", _ssoTokenRequestData.ClientIp);
        
        

        throw new MissingFieldException("MissingRequestData: " + data);
      }
    }
  }
}
