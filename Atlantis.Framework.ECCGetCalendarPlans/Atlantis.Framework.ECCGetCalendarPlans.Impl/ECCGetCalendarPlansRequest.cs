using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetCalendarPlans.Impl.Json;
using Atlantis.Framework.ECCGetCalendarPlans.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetCalendarPlans.Impl
{
  public class ECCGetCalendarPlansRequest : IRequest
  {
    private const string REQUEST_METHOD = "getCalendarPlanInfoForShopper";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetCalendarPlansRequestData eccGetCalendarPlansRequestData = (ECCGetCalendarPlansRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetCalendarPlansResponseData(requestData, new AuthenticationException("Unable to authenticate ECCGetCalendarPlanInfoForShopperRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          EccJsonGetCalendarPlansRequest jsonRequestBody = new EccJsonGetCalendarPlansRequest();
          jsonRequestBody.ShopperId = eccGetCalendarPlansRequestData.ShopperID;
          jsonRequestBody.ResellerId = eccGetCalendarPlansRequestData.PrivateLabelId.ToString();
          jsonRequestBody.PlanUid = eccGetCalendarPlansRequestData.CalendarPlanUid;
          jsonRequestBody.SubAccount = eccGetCalendarPlansRequestData.SubAccount;

          EccJsonRequest<EccJsonGetCalendarPlansRequest> jsonRequest = new EccJsonRequest<EccJsonGetCalendarPlansRequest>
                                                                         {
                                                                           Id = authName,
                                                                           Token = authToken,
                                                                           Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, "AccountUid", "ASC"),
                                                                           Parameters = jsonRequestBody
                                                                         };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey);
          responseData = new ECCGetCalendarPlansResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetCalendarPlansResponseData(requestData, ex);
        }
      }
      return responseData;
    }

    #endregion
  }
}
