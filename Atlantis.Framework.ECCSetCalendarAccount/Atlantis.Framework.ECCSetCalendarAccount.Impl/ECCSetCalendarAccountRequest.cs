using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCSetCalendarAccount.Impl.Json;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ECCSetCalendarAccount.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCSetCalendarAccount.Impl
{
  public class ECCSetCalendarAccountRequest : IRequest
  {
    #region Implementation of IRequest

    private const string REQUEST_METHOD = "setCalendarAccount";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCSetCalendarAccountRequestData eccSetCalendarAccountRequestData = (ECCSetCalendarAccountRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCSetCalendarAccountResponseData(requestData, new AuthenticationException("Unable to authenticate ECCSetCalendarAccountRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          EccJsonSetCalendarAccountRequest jsonRequestBody = new EccJsonSetCalendarAccountRequest();
          jsonRequestBody.ShopperId = eccSetCalendarAccountRequestData.ShopperID;
          jsonRequestBody.ResellerId = eccSetCalendarAccountRequestData.PrivateLabelId.ToString();
          jsonRequestBody.EmailAddress = eccSetCalendarAccountRequestData.EmailAddress;
          jsonRequestBody.CalendarPlanUid = eccSetCalendarAccountRequestData.CalendarPlanGuid.ToString();
          jsonRequestBody.InviteMessage = eccSetCalendarAccountRequestData.Message;
          jsonRequestBody.SubAccount = eccSetCalendarAccountRequestData.SubAccount;

          EccJsonRequest<EccJsonSetCalendarAccountRequest> jsonRequest = new EccJsonRequest<EccJsonSetCalendarAccountRequest>
                                                                           {
                                                                             Id = authName,
                                                                             Token = authToken,
                                                                             Return=new EccJsonPaging(PAGE_NUMBER,RESULTS_PER_PAGE,string.Empty,"ASC"),
                                                                             Parameters=jsonRequestBody
                                                                           };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey);
          responseData = new ECCSetCalendarAccountResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCSetCalendarAccountResponseData(requestData, ex);
        }
      }
      return responseData;
    }

    #endregion
  }
}
