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

        var byteData = GetTokenRequestByteData();
        var wsUrl = ((WsConfigElement)config).WSURL;

        var tokenWebRequest = HttpHelpers.GetHttpWebRequestAndAddByteData(wsUrl, byteData, "application/x-www-form-urlencoded", "POST");
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

    private byte[] GetTokenRequestByteData()
    {
      var inputDataString = new StringBuilder(100);

      inputDataString.Append("factor=p_sms");
      inputDataString.Append("&token=");
      inputDataString.Append(_ssoTokenRequestData.EncryptedToken);
      inputDataString.Append("&value=");
      inputDataString.Append(_ssoTokenRequestData.TwoFactorCode);
      
      var byteData = Encoding.UTF8.GetBytes(inputDataString.ToString());
      return byteData;
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
