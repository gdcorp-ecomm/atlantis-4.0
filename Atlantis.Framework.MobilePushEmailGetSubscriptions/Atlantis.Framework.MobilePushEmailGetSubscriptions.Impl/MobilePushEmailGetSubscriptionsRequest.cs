using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.MobilePushEmailGetSub.Interface;
using Atlantis.Framework.MobilePushEmailGetSubscriptions.Impl.Json;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MobilePushEmailGetSubscriptions.Impl
{
  public class MobilePushEmailGetSubscriptionsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
        /*
        Retrieve the RIM status for an email address

        @param RequestObj $aRequest   Standard inbound request object
        REQUIRED OBJECT PROPERTIES:
        RequestObj->Id                          The Authentication ID used to identify the caller
        RequestObj->Token                       The Token/Password associated with the supplied Authentication ID
        RequestObj->Parameters->emailaddress    The email address being queried

        CONTITIONAL OBJECT PROPERTIES:
        RequestObj->Parameters->subid           The Subscription ID.
                                              If not supplied, returns all records.
                                              If supplied, restricts results to only
                                              the RIM.Subscription record with a matching subscription_id.



        OPTIONAL OBJECT PROPERTIES:
        RequestObj->Parameters->subaccount      The Shopper ID of the customer sub account being queried (overrides the shopper)

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
        ResponseObj->Results        = An indexed array of Subscription IDs established for the email address

        ON FAIL:
        ResponseObj->ResultCode     = Unique number indicating why the web service failed
        ResponseObj->Message        = Reason why web service failed
        ResponseObj->Timer          = # of seconds it took to derive the response
        ResponseObj->Results        = empty
        */

      IResponseData responseData = null;

      var subscribeRequest = (MobilePushEmailGetSubscriptionsRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "getRIMForEmail";

      string requestKey;
      string authName;
      string authToken;


      MobilePushEmailGetSubscriptionsRequestData mobPushGetSubsRequest = (MobilePushEmailGetSubscriptionsRequestData)oRequestData;
      WsConfigElement wsConfig = (WsConfigElement)oConfig;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        if (string.IsNullOrEmpty(mobPushGetSubsRequest.Email))
        {
          throw new Exception("Email must be present in parameters");
        }

        var oJsonRequestBody = new MobilePushEmailGetSubscriptionJsonRequest
        {
          EmailAddress = subscribeRequest.Email,
          Subaccount = string.Empty,
          SubscriptionId = subscribeRequest.SubscriptionId
        };

        var oRequest = new EccJsonRequest<MobilePushEmailGetSubscriptionJsonRequest>
        {
          Id = authName,
          Token = authToken,
          Return = new EccJsonPaging(pageNumber, resultsPerPage, string.Empty, string.Empty),
          Parameters = oJsonRequestBody
        };

        string sRequest = System.Uri.EscapeDataString(oRequest.ToJson());
        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new MobilePushEmailGetSubscriptionsResponseData(response);

      }
      catch (Exception ex)
      {
        responseData = new MobilePushEmailGetSubscriptionsResponseData(oRequestData, ex);
      }

      return responseData;
    }
  }
}
