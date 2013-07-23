using System;
using System.Globalization;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobilePushEmailUnsubscribe.Impl.Json;
using Atlantis.Framework.MobilePushEmailUnsubscribe.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MobilePushEmailUnsubscribe.Impl
{
  public class MobilePushEmailUnsubscribeRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      /*
Unsubscribe an Email Address from RIM

 @param RequestObj $aRequest   Standard inbound request object
   REQUIRED OBJECT PROPERTIES:
       RequestObj->Id                          The Authentication ID used to identify the caller
       RequestObj->Token                       The Token/Password associated with the supplied Authentication ID
       RequestObj->Parameters->emailaddress    The email address being unsubscribed from RIM
       RequestObj->Parameters->subscription    The 'X-Subscription-Id' value from the original request headers from the customer

   CONTITIONAL OBJECT PROPERTIES:
       None of the RequestObj properties are conditional for this web service

   OPTIONAL OBJECT PROPERTIES:
       RequestObj->Parameters->subaccount      The Shopper ID of the customer sub account being queried (overrides the shopper)
       RequestObj->Parameters->mobile          Flag to force "mobile" bit in EmailAuth.UserInfo table
                                               1 = Set mobile bit ON
                                               0 = Set mobile bit OFF
                                               Not provided = Ignored

   IGNORED OBJECT PROPERTIES:
       RequestObj->Parameters->shopper         The Shopper ID of the customer account being queried
       RequestObj->Parameters->reseller        The Reseller ID associated to the Shopper ID
       RequestObj->Return->PageNumber
       RequestObj->Return->ResultsPerPage
       RequestObj->Return->OrderBy
       RequestObj->Return->SortOrder

 @return ResponseObj       Standard outbound response object
   ON SUCCESS:
       ResponseObj->ResultCode     = 0
       ResponseObj->Message        = blank
       ResponseObj->Timer          = # of seconds it took to derive the response
       ResponseObj->Results        = empty

   ON FAIL:
       ResponseObj->ResultCode     = Unique number indicating why the web service failed
       ResponseObj->ResultCodes     = 30000; message = "Email address xxx does not exist." 
  30001; message = "Email address xxx is a type of: zzz. Only email accounts may be unsubscribed from RIM." 
 30002; message = "The email address xxx already exists." 
 30003; message = "3 Failed to unsubscribe xxx / zzz" 
 30004; message = "2 Failed to unsubscribe xxx / yyy"
       ResponseObj->Message        = Reason why web service failed
       ResponseObj->Timer          = # of seconds it took to derive the response
       ResponseObj->Results        = empty
       * */

      MobilePushEmailUnsubscribeRequestData mobilePushEmailUnsubscribeRequestData = (MobilePushEmailUnsubscribeRequestData)oRequestData;
      WsConfigElement wsConfigElement = (WsConfigElement)oConfig;

      if (mobilePushEmailUnsubscribeRequestData.SubscriptionId <= 0)
      {
        throw new Exception("SubscriptionId must be greater than zero.");
      }

      if (string.IsNullOrEmpty(mobilePushEmailUnsubscribeRequestData.Email))
      {
        throw new Exception("Email cannot be empty.");
      }


      IResponseData responseData;
      var unsubscribeRequest = (MobilePushEmailUnsubscribeRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "removeRIMAccount";

      string requestKey; //= "tH15!zt433Cc@P1*";
      string authName; // = "GDMobile201111";
      string authToken;// = "DgD11M0b!l32o!1";

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        var wsConfig = ((WsConfigElement)oConfig);

        var oJsonRequestBody = new MobilePushEmailUnsubscribeJsonRequest
        {
          EmailAddress = unsubscribeRequest.Email,
          IsMobile = (unsubscribeRequest.IsMobile.HasValue ? (unsubscribeRequest.IsMobile.Value ? "1" : "0") : string.Empty),
          Subaccount = string.Empty,
          SubscriptionId = unsubscribeRequest.SubscriptionId.ToString(CultureInfo.InvariantCulture)
        };

        var oRequest = new EccJsonRequest<MobilePushEmailUnsubscribeJsonRequest>
        {
          Id = authName,
          Token = authToken,
          Return = new EccJsonPaging(pageNumber, resultsPerPage, string.Empty, string.Empty),
          Parameters = oJsonRequestBody
        };

        string sRequest = System.Uri.EscapeDataString(oRequest.ToJson());
        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new MobilePushEmailUnsubscribeResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new MobilePushEmailUnsubscribeResponseData(oRequestData, ex);
      }

      return responseData;

    }
  }
}
