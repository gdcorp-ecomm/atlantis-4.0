using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetAddressesForShopper.Impl.Json;
using Atlantis.Framework.ECCGetAddressesForShopper.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetAddressesForShopper.Impl
{
  public class ECCGetAddressesForShopperRequest : IRequest
  {
    private const string REQUEST_METHOD = "getEmailAddressesForShopper";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetAddressesForShopperRequestData eccGetAddressesForShopperRequestData = (ECCGetAddressesForShopperRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetAddressesForShopperResponseData(requestData, new AuthenticationException("Unable to authenticate ECCGetAddressesForShopperRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          EccJsonGetAddressesForShopperRequest jsonRequestBody = new EccJsonGetAddressesForShopperRequest();
          jsonRequestBody.ShopperId = eccGetAddressesForShopperRequestData.ShopperID;
          jsonRequestBody.ResellerId = eccGetAddressesForShopperRequestData.PrivateLabelId.ToString();
          jsonRequestBody.EmailType = eccGetAddressesForShopperRequestData.EmailType.ToString();
          jsonRequestBody.Active = eccGetAddressesForShopperRequestData.Active.ToString();
          jsonRequestBody.SubAccount = eccGetAddressesForShopperRequestData.SubAccount;

          EccJsonRequest<EccJsonGetAddressesForShopperRequest> jsonRequest = new EccJsonRequest<EccJsonGetAddressesForShopperRequest>
                                                                               {
                                                                                 Id = authName,
                                                                                 Token = authToken,
                                                                                 Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, string.Empty, string.Empty),
                                                                                 Parameters = jsonRequestBody
                                                                               };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey,eccGetAddressesForShopperRequestData.RequestTimeout);
          responseData = new ECCGetAddressesForShopperResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetAddressesForShopperResponseData(requestData, ex);
        }
      }
      return responseData;
    }

    #endregion
  }
}
