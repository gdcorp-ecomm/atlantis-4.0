using System;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetOFFPlanInfo.Impl.Json;
using Atlantis.Framework.ECCGetOFFPlanInfo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetOFFPlanInfo.Impl
{
  public class ECCGetOFFPlanInfoRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData;
      ECCGetOFFPlanInfoRequestData plansRequest = (ECCGetOFFPlanInfoRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "getOFFPlanInfoForShopper";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        ECCJsonGetOFFPlanInfoRequest oJsonRequestBody = new ECCJsonGetOFFPlanInfoRequest();
        oJsonRequestBody.ShopperId = plansRequest.ShopperID;
        oJsonRequestBody.ResellerId = plansRequest.ResellerId.ToString();
        oJsonRequestBody.AccountUid = plansRequest.AccountUid;
        oJsonRequestBody.SubAccount = plansRequest.Subaccount;
        oJsonRequestBody.Status = plansRequest.Status;

        EccJsonRequest<ECCJsonGetOFFPlanInfoRequest> oRequest = new EccJsonRequest<ECCJsonGetOFFPlanInfoRequest>();
        oRequest.Id = authName;
        oRequest.Token = authToken;
        oRequest.Return = new EccJsonPaging(pageNumber, resultsPerPage, String.Empty, string.Empty);
        oRequest.Parameters = oJsonRequestBody;

        string sRequest = oRequest.ToJson();

        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new ECCGetOFFPlanInfoResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new ECCGetOFFPlanInfoResponseData(oRequestData, ex);
      }

      return responseData;
    }
  }
}
