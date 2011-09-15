using System;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetEmailPlanInfo.Interface;
using Atlantis.Framework.ECCGetEmailPlanInfoForShopper.Impl.Json;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetEmailPlanInfo.Impl
{
  public class ECCGetEmailPlanInfoRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData;
      var plansRequest = (EccGetEmailPlanInfoRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "getEmailPlanInfoForShopper";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        var wsConfig = ((WsConfigElement)oConfig);

        var oJsonRequestBody = new EccJsonEmailPlanInfoRequest
                                 {
                                   EmailType = ((int) plansRequest.EmailType).ToString(),
                                   ShopperId = plansRequest.ShopperID,
                                   ResellerId = plansRequest.ResellerId.ToString(),
                                   AccountUid = plansRequest.AccountUid,
                                   SubAccount = plansRequest.Subaccount,
                                   DeepLoad = plansRequest.DeepLoad
                                 };

        var oRequest = new EccJsonRequest<EccJsonEmailPlanInfoRequest>
                         {
                           Id = authName,
                           Token = authToken,
                           Return = new EccJsonPaging(pageNumber, resultsPerPage, string.Empty, string.Empty),
                           Parameters = oJsonRequestBody
                         };

        string sRequest = oRequest.ToJson();

        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey, plansRequest.RequestTimeout);
        responseData = new EccGetEmailPlanInfoResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new EccGetEmailPlanInfoResponseData(oRequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}
