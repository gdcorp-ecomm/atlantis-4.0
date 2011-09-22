using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetAddressesForPlanEX.Impl.Json;
using Atlantis.Framework.ECCGetAddressesForPlanEX.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetAddressesForPlanEX.Impl
{
  public class ECCGetAddressesForPlanEXRequest : IRequest
  {
    private const string REQUEST_METHOD = "getEmailAddressesForPlan";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetAddressesForPlanEXRequestData eccGetAddressesForPlanExRequestData = (ECCGetAddressesForPlanEXRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetAddressesForPlanEXResponseData(requestData, new AuthenticationException("Unable to authenticate ECCGetAddressesForPlanEXRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          ECCJsonGetAddressesForPlanEXRequest jsonExRequestBody = new ECCJsonGetAddressesForPlanEXRequest();
          jsonExRequestBody.ShopperId = eccGetAddressesForPlanExRequestData.ShopperID;
          jsonExRequestBody.ResellerId = eccGetAddressesForPlanExRequestData.PrivateLabelId.ToString();
          jsonExRequestBody.AccountUid = eccGetAddressesForPlanExRequestData.AccountUid;
          jsonExRequestBody.IncludeActiveOnly = eccGetAddressesForPlanExRequestData.Active;

          if (eccGetAddressesForPlanExRequestData.Fields != null)
          {
            jsonExRequestBody.Fields = string.Join(",", eccGetAddressesForPlanExRequestData.Fields.ToArray());
          }
          else
          {
            jsonExRequestBody.Fields = "status,delivery_mode";
          }

          EccJsonRequest<ECCJsonGetAddressesForPlanEXRequest> jsonRequest = new EccJsonRequest<ECCJsonGetAddressesForPlanEXRequest>
          {
            Id = authName,
            Token = authToken,
            Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, string.Empty, string.Empty),
            Parameters = jsonExRequestBody
          };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey, eccGetAddressesForPlanExRequestData.RequestTimeout);
          responseData = new ECCGetAddressesForPlanEXResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetAddressesForPlanEXResponseData(requestData, ex);
        }
      }
      return responseData;
    }

    #endregion
  }
}
