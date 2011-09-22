using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetCalendarDetails.Impl.Json;
using Atlantis.Framework.ECCGetCalendarDetails.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetCalendarDetails.Impl
{
  public class ECCGetCalendarDetailsRequest : IRequest
  {
    private const string REQUEST_METHOD = "getCalendarAccountsForShopper";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetCalendarDetailsRequestData calendarAccountsRequest = (ECCGetCalendarDetailsRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetCalendarDetailsResponseData(requestData,
                                                              new AuthenticationException(
                                                                "Unable to autheticate ECCGetAutoResponderRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          ECCJsonGetCalendarDetailsRequest jsonRequestBody = new ECCJsonGetCalendarDetailsRequest();
          jsonRequestBody.ShopperId = calendarAccountsRequest.ShopperID;
          jsonRequestBody.ResellerId = calendarAccountsRequest.PrivateLabelId.ToString();
          jsonRequestBody.IncludeActiveOnly = calendarAccountsRequest.IncludeActiveOnly;
          jsonRequestBody.EmailAddress = calendarAccountsRequest.EmailAddress;
          jsonRequestBody.SubAccount = calendarAccountsRequest.SubAccount;

          EccJsonRequest<ECCJsonGetCalendarDetailsRequest> jsonRequest = new EccJsonRequest
            <ECCJsonGetCalendarDetailsRequest>
          {
            Id = authName,
            Token = authToken,
            Return =
              new EccJsonPaging(PAGE_NUMBER,
                                RESULTS_PER_PAGE,
                                "ar_from", "ASC"),
            Parameters = jsonRequestBody
          };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey);
          responseData = new ECCGetCalendarDetailsResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetCalendarDetailsResponseData(requestData, ex);
        }
      }

      return responseData;
    }
  }
}
