using System;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetSmtpRelayInfo.Impl.Json;
using Atlantis.Framework.ECCGetSmtpRelayInfo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetSmtpRelayInfo.Impl
{
  public class ECCGetSmtpRelayInfoRequest : IRequest 
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      // 
      IResponseData responseData;
      ECCGetSmtpRelayInfoRequestData plansRequest = (ECCGetSmtpRelayInfoRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "getSMTPRelayInfoForShopper";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        EccJsonSmtpRelayInfoRequest oJsonRequestBody = new EccJsonSmtpRelayInfoRequest();
        oJsonRequestBody.EmailType = ((int)plansRequest.EmailType).ToString();
        oJsonRequestBody.ShopperId = plansRequest.ShopperID;
        oJsonRequestBody.ResellerId = plansRequest.ResellerId.ToString();

        EccJsonRequest<EccJsonSmtpRelayInfoRequest> oRequest = new EccJsonRequest<EccJsonSmtpRelayInfoRequest>();
        oRequest.Id = authName;
        oRequest.Token = authToken;
        oRequest.Return = new EccJsonPaging(pageNumber, resultsPerPage, String.Empty, string.Empty);
        oRequest.Parameters = oJsonRequestBody;

        string sRequest = oRequest.ToJson();

        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new ECCGetSmtpRelayInfoResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new ECCGetSmtpRelayInfoResponseData(oRequestData, ex);
      }

      return responseData;
    }
  }
}
