using System;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.EccRenameEmailPlan.Impl.Json;
using Atlantis.Framework.EccRenameEmailPlan.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.EccRenameEmailPlan.Impl
{
  public class EccRenameEmailPlanRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData;
      EccRenameEmailPlanRequestData plansRequest = (EccRenameEmailPlanRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "renameEmailPlan";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        EccJsonRenameEmailPlanRequest oJsonRequestBody = new EccJsonRenameEmailPlanRequest();
        oJsonRequestBody.ShopperId = plansRequest.ShopperID;
        oJsonRequestBody.ResellerId = plansRequest.ResellerId;
        oJsonRequestBody.AccountUid = new Guid(plansRequest.AccountUid);
        oJsonRequestBody.NewPlanName = plansRequest.NewPlanName;
        oJsonRequestBody.Subaccount = plansRequest.Subaccount;

        EccJsonRequest<EccJsonRenameEmailPlanRequest> oRequest = new EccJsonRequest<EccJsonRenameEmailPlanRequest>();
        oRequest.Id = authName;
        oRequest.Token = authToken;
        oRequest.Return = new EccJsonPaging(pageNumber, resultsPerPage, String.Empty, string.Empty);
        oRequest.Parameters = oJsonRequestBody;

        string sRequest = oRequest.ToJson();

        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new EccRenameEmailPlanResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new EccRenameEmailPlanResponseData(oRequestData, ex);
      }

      return responseData;
    }
  }
}
