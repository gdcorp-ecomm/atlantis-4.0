using System;
using System.Xml;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.EccGetEmailAddressInfo.Impl.Json;
using Atlantis.Framework.EccGetEmailAddressInfo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.EccGetEmailAddressInfo.Impl
{
  public class EccGetEmailAddressInfoRequest : IRequest 
  {
    #region Implementation of IRequest
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData;
      EccGetEmailAddressInfoRequestData plansRequest = (EccGetEmailAddressInfoRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "getEmailAddressInfoForShopper";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);     

      try
      {
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        EccJsonGetEmailAccountInfoRequest oJsonRequestBody = new EccJsonGetEmailAccountInfoRequest();
        oJsonRequestBody.Subaccount = plansRequest.Subaccount;
        oJsonRequestBody.ShopperId = plansRequest.ShopperID;
        oJsonRequestBody.EmailAddress = plansRequest.EmailAddress;
        oJsonRequestBody.IncludeActiveOnly = plansRequest.IncludeActiveOnly;
        oJsonRequestBody.IncludeDynamicData = plansRequest.IncludeDynamicData;
        oJsonRequestBody.ResellerId = plansRequest.ResellerId;
        oJsonRequestBody.Type = plansRequest.Type;
        
        EccJsonRequest<EccJsonGetEmailAccountInfoRequest> oRequest = new EccJsonRequest<EccJsonGetEmailAccountInfoRequest>();
        oRequest.Id = authName;
        oRequest.Token = authToken;
        oRequest.Return = new EccJsonPaging(pageNumber, resultsPerPage, String.Empty, string.Empty);
        oRequest.Parameters = oJsonRequestBody;

        string sRequest = oRequest.ToJson();

        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new EccGetEmailAddressInfoResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new EccGetEmailAddressInfoResponseData(oRequestData, ex);
      }

      return responseData;
    }


    #endregion
  }
}
