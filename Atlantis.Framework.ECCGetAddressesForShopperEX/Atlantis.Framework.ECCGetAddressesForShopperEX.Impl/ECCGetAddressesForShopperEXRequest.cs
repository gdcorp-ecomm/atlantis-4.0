using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetAddressesForShopperEX.Impl.Json;
using Atlantis.Framework.ECCGetAddressesForShopperEX.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetAddressesForShopperEX.Impl
{
  public class ECCGetAddressesForShopperEXRequest : IRequest
  {
    private const string REQUEST_METHOD = "getEmailAddressesForShopper";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetAddressesForShopperEXRequestData eccGetAddressesForShopperRequestData = (ECCGetAddressesForShopperEXRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetAddressesForShopperEXResponseData(requestData, new AuthenticationException("Unable to authenticate ECCGetAddressesForShopperRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          EccJsonGetAddressesForShopperEXRequest jsonExRequestBody = new EccJsonGetAddressesForShopperEXRequest();
          jsonExRequestBody.ShopperId = eccGetAddressesForShopperRequestData.ShopperID;
          jsonExRequestBody.ResellerId = eccGetAddressesForShopperRequestData.PrivateLabelId.ToString();
          jsonExRequestBody.EmailType = eccGetAddressesForShopperRequestData.EmailType.ToString();
          jsonExRequestBody.Active = eccGetAddressesForShopperRequestData.Active.ToString();
          jsonExRequestBody.SubAccount = eccGetAddressesForShopperRequestData.SubAccount;

          if (eccGetAddressesForShopperRequestData.Fields != null)
          {
            jsonExRequestBody.Fields = string.Join(",", eccGetAddressesForShopperRequestData.Fields.ToArray());
          }
          else
          {
            jsonExRequestBody.Fields = "status,delivery_mode";
          }

          EccJsonRequest<EccJsonGetAddressesForShopperEXRequest> jsonRequest = new EccJsonRequest<EccJsonGetAddressesForShopperEXRequest>
          {
            Id = authName,
            Token = authToken,
            Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, string.Empty, string.Empty),
            Parameters = jsonExRequestBody
          };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey, eccGetAddressesForShopperRequestData.RequestTimeout);
          responseData = new ECCGetAddressesForShopperEXResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetAddressesForShopperEXResponseData(requestData, ex);
        }
      }
      return responseData;
    }

    #endregion
  }
}
