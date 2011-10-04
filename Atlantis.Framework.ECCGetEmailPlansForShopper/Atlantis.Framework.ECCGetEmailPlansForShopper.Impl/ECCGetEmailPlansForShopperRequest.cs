using System;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetEmailPlansForShopper.Impl.Json;
using Atlantis.Framework.ECCGetEmailPlansForShopper.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetEmailPlansForShopper.Impl
{
  public class ECCGetEmailPlansForShopperRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData;
      var plansRequest = (ECCGetEmailPlansForShopperRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "getEmailPlansForShopper";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);
      
      try
      {
        var wsConfig = ((WsConfigElement)oConfig);

        var oJsonRequestBody = new EccJsonEmailPlansForShopperRequest();
        oJsonRequestBody.EmailType = ((int)plansRequest.EmailType).ToString();
        oJsonRequestBody.ShopperId = plansRequest.ShopperID;
        oJsonRequestBody.ResellerId = plansRequest.ResellerId.ToString();

        var oRequest = new EccJsonRequest<EccJsonEmailPlansForShopperRequest>
                         {
                           Id = authName,
                           Token = authToken,
                           Return = new EccJsonPaging(pageNumber, resultsPerPage, "pack_name", "ASC"),
                           Parameters = oJsonRequestBody
                         };

        string sRequest = oRequest.ToJson();
        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new ECCGetEmailPlansForShopperResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new ECCGetEmailPlansForShopperResponseData(oRequestData, ex);
      }

      return responseData;
    }

    #endregion
  }

}
