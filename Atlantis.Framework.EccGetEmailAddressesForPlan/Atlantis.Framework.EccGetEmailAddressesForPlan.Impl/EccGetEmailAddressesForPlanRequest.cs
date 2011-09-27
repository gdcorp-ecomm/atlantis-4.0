using System;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.EccGetEmailAddressesForPlan.Impl.Json;
using Atlantis.Framework.EccGetEmailAddressesForPlan.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.EccGetEmailAddressesForPlan.Impl
{
  public class EccGetEmailAddressesForPlanRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData;
      EccGetEmailAddressesForPlanRequestData plansRequest = (EccGetEmailAddressesForPlanRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "getEmailAddressesForPlan";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        EccJsonGetEmailAddressesForPlanRequest oJsonRequestBody = new EccJsonGetEmailAddressesForPlanRequest();
        oJsonRequestBody.ShopperId = plansRequest.ShopperID;
        oJsonRequestBody.ResellerId = plansRequest.ResellerId.ToString();
        oJsonRequestBody.IncludeActiveOnly = plansRequest.IncludeActiveOnly;
        oJsonRequestBody.AccountUid = new Guid(plansRequest.AccountUid);

        EccJsonRequest<EccJsonGetEmailAddressesForPlanRequest> oRequest = new EccJsonRequest<EccJsonGetEmailAddressesForPlanRequest>();
        oRequest.Id = authName;
        oRequest.Token = authToken;
        oRequest.Return = new EccJsonPaging(pageNumber, resultsPerPage, String.Empty, string.Empty);
        oRequest.Parameters = oJsonRequestBody;

        string sRequest = oRequest.ToJson();

        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new EccGetEmailAddressesForPlanResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new EccGetEmailAddressesForPlanResponseData(oRequestData, ex);
      }

      return responseData;
    }
  }
}
