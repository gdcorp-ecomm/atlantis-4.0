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
