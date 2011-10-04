using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetOFFAccountsForShopper.Impl.Json;
using Atlantis.Framework.ECCGetOFFAccountsForShopper.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetOFFAccountsForShopper.Impl
{
  public class ECCGetOFFAccountsForShopperRequest : IRequest
  {
    private const string REQUEST_METHOD = "getOFFAccountsForShopper";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetOFFAccountsForShopperRequestData eccGetEmailAddressesForDomainRequestData = (ECCGetOFFAccountsForShopperRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetOFFAccountsForShopperResponseData(requestData, new AuthenticationException("Unable to autheticate ECCGetEmailAddressesForDomainRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          ECCJsonGetOFFAccountsForShopperRequest jsonRequestBody = new ECCJsonGetOFFAccountsForShopperRequest();
          jsonRequestBody.ShopperId = eccGetEmailAddressesForDomainRequestData.ShopperID;
          jsonRequestBody.ResellerId = eccGetEmailAddressesForDomainRequestData.PrivateLabelId.ToString();
          jsonRequestBody.Username = eccGetEmailAddressesForDomainRequestData.Username;
          jsonRequestBody.EmailAddress= eccGetEmailAddressesForDomainRequestData.EmailAddress;
          jsonRequestBody.ActiveOnly = eccGetEmailAddressesForDomainRequestData.ActiveOnly;

          EccJsonRequest<ECCJsonGetOFFAccountsForShopperRequest> jsonRequest = new EccJsonRequest<ECCJsonGetOFFAccountsForShopperRequest>
          {
            Id = authName,
            Token = authToken,
            Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, "ar_from", "ASC"),
            Parameters = jsonRequestBody
          };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey);
          responseData = new ECCGetOFFAccountsForShopperResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetOFFAccountsForShopperResponseData(requestData, ex);
        }
      }

      return responseData;
    }
  }
}
