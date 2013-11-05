using Atlantis.Framework.Interface;
using Atlantis.Framework.Sso.Impl.Helpers;
using Atlantis.Framework.Sso.Interface;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;
using System;

namespace Atlantis.Framework.Sso.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.Sso.Impl.SsoGetKeyRequest" assembly="Atlantis.Framework.Sso.Impl.dll" request_type="###" />

  public class SsoGetKeyRequest : IRequest
  {
    private SsoGetKeyRequestData _ssoKeyRequestData;
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoGetKeyResponseData responseData;

      try
      {
         _ssoKeyRequestData = (SsoGetKeyRequestData)requestData;
        ThrowExceptionIfRequestDataIsMissing();

        var wsUrl = ((WsConfigElement)config).WSURL;
        var wsUrlWithKey = string.Concat(wsUrl, _ssoKeyRequestData.Token.Header.kid);

        var tokenWebRequest = HttpHelpers.GetHttpWebRequestAndAddData(wsUrlWithKey, null, "text/xhtml", "GET");

        Key key = HttpHelpers.GetWebResponseAndConvertToObject<Key>(tokenWebRequest);
        responseData = new SsoGetKeyResponseData(key, _ssoKeyRequestData.Token);
      }
      catch (AtlantisException aex)
      {
        responseData = new SsoGetKeyResponseData(aex);
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(requestData, "SsoGetKeyRequest::RequestHandler", ex.Message, "requestType: " + config.RequestType);
        responseData = new SsoGetKeyResponseData(aex);
      }

      return responseData;
    }


    private void ThrowExceptionIfRequestDataIsMissing()
    {
      var isValid = !string.IsNullOrEmpty(_ssoKeyRequestData.Token.Header.kid) && !string.IsNullOrEmpty(_ssoKeyRequestData.Token.RawTokenData.Signature);

      if (!isValid)
      {
        var data = string.Concat("kid: ", _ssoKeyRequestData.Token.Header.kid, " | RawTokenRequest: ", _ssoKeyRequestData.Token.RawTokenData);
        throw new MissingFieldException("MissingRequestData: " + data);
      }
    }
  }
}
