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
  public class SsoValidateTwoFactorRequest : IRequest
  {
    private SsoValidateTwoFactorRequestData _ssoTokenRequestData;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoValidateTwoFactorResponseData response;

      try
      {
        _ssoTokenRequestData = (SsoValidateTwoFactorRequestData)requestData;
        ThrowExceptionIfRequestDataIsMissing();

        var wsConfigElement = ((WsConfigElement)config);

        var urlEncodedData = GetTokenRequestData();
        var wsUrl = wsConfigElement.WSURL;
        X509Certificate2 clientCert = wsConfigElement.GetClientCertificate();

        var tokenWebRequest = HttpHelpers.GetHttpWebRequestAndAddData(wsUrl, urlEncodedData, "application/x-www-form-urlencoded", "POST", clientCert);
        Token token = HttpHelpers.GetWebResponseAndConvertToObject<Token>(tokenWebRequest);

        response = new SsoValidateTwoFactorResponseData(token);
      }
      catch (AtlantisException aex)
      {
        response = new SsoValidateTwoFactorResponseData(aex);
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(requestData, "SsoValidateTwoFactorToken::RequestHandler", ex.Message, "requestType: " + config.RequestType);
        response = new SsoValidateTwoFactorResponseData(aex);
      }

      return response;
    }

    private string GetTokenRequestData()
    {
      var inputDataString = new StringBuilder(100);

      inputDataString.Append("factor=p_sms");
      inputDataString.Append("&token=");
      inputDataString.Append(HttpUtility.UrlEncode(_ssoTokenRequestData.EncryptedToken));
      inputDataString.Append("&value=");
      inputDataString.Append(HttpUtility.UrlEncode(_ssoTokenRequestData.TwoFactorCode));

      return inputDataString.ToString();
    }

    private void ThrowExceptionIfRequestDataIsMissing()
    {
      var isValid = !string.IsNullOrEmpty(_ssoTokenRequestData.EncryptedToken)
                    && !string.IsNullOrEmpty(_ssoTokenRequestData.TwoFactorCode);

      if (!isValid)
      {
        var data = string.Concat("EncryptedToken: ", _ssoTokenRequestData.EncryptedToken,
          " | TwoFactorCode: ", _ssoTokenRequestData.TwoFactorCode);

        throw new MissingFieldException("MissingRequestData: " + data);
      }
    }
  }
}
