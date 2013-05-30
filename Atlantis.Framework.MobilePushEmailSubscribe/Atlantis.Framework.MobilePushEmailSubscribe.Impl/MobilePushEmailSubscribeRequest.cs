using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobilePushEmailSubscribe.Impl.Json;
using Atlantis.Framework.MobilePushEmailSubscribe.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MobilePushEmailSubscribe.Impl
{
  public class MobilePushEmailSubscribeRequest : IRequest
  {
    private static void PostRequest(string requestUrl, TimeSpan requestTimeout, IEnumerable<KeyValuePair<string, string>> requestHeaders, out IDictionary<string, string> responseHeaders)
    {
      responseHeaders = new Dictionary<string, string>(16);

      try
      {
        HttpWebRequest webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;

        if (webRequest != null)
        {
          webRequest.Timeout = (int)requestTimeout.TotalMilliseconds;
          webRequest.Method = "POST";
          webRequest.ContentType = "application/x-www-form-urlencoded";
          webRequest.ContentLength = 0;

          foreach (KeyValuePair<string, string> customRequestHeader in requestHeaders)
          {
            webRequest.Headers.Add(customRequestHeader.Key, customRequestHeader.Value);
          }
        }

        if (webRequest != null)
        {
          using (HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse)
          {
            if (webResponse != null && webRequest.HaveResponse)
            {
              using (Stream webResponseData = webResponse.GetResponseStream())
              {
                if (webResponseData != null)
                {
                  foreach (string headerKey in webResponse.Headers.AllKeys)
                  {
                    responseHeaders[headerKey] = webResponse.Headers[headerKey];
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error posting to ECC subscribe service. {0}", ex.Message));
      }
    }

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData;
      var subscribeRequest = (MobilePushEmailSubscribeRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "setRIMAccount";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        var wsConfig = ((WsConfigElement)oConfig);

        if (string.IsNullOrEmpty(subscribeRequest.CallbackUrl))
        {
          throw new Exception("CallbackUrl cannot be empty.");
        }

        if (string.IsNullOrEmpty(subscribeRequest.PushRegistrationId))
        {
          throw new Exception("PushRegistrationId cannot be empty.");
        }

        if (string.IsNullOrEmpty(subscribeRequest.Email))
        {
          throw new Exception("Email cannot be empty.");
        }

        string callBackUrl = string.Format("{0}?action=Notification&login={1}", subscribeRequest.CallbackUrl,
                                                                                subscribeRequest.Email);
        
        var oJsonRequestBody = new MobilePushEmailSubscribeJsonRequest
                                 {
                                   Notification = subscribeRequest.PushRegistrationId,
                                   Callback = callBackUrl,
                                   EmailAddress = subscribeRequest.Email,
                                   IsMobile = (subscribeRequest.IsMobile.HasValue ? (subscribeRequest.IsMobile.Value ? "1" : "0")  : string.Empty),
                                   Subaccount = string.Empty
                                 };

        var oRequest = new EccJsonRequest<MobilePushEmailSubscribeJsonRequest>
        {
          Id = authName,
          Token = authToken,
          Return = new EccJsonPaging(pageNumber, resultsPerPage, string.Empty, string.Empty),
          Parameters = oJsonRequestBody
        };

        string sRequest = System.Uri.EscapeDataString(oRequest.ToJson());
        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new MobilePushEmailSubscribeResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new MobilePushEmailSubscribeResponseData(oRequestData, ex);
      }

      return responseData;
    }
    #endregion
  }
}
