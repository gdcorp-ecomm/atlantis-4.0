using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetProductsForEmailAddress.Impl.Json;
using Atlantis.Framework.ECCGetProductsForEmailAddress.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetProductsForEmailAddress.Impl
{
  public class ECCGetProductsForEmailAddressRequest : IRequest
  {
    private const string REQUEST_METHOD = "getProductsForEmailAddress";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetProductsForEmailAddressRequestData eccGetProductsForEmailAddressRequestData = (ECCGetProductsForEmailAddressRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetProductsForEmailAddressResponseData(requestData, new AuthenticationException("Unable to autheticate ECCGetProductsForEmailAddressRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          ECCJsonGetProductsForEmailAddressRequest jsonRequestBody = new ECCJsonGetProductsForEmailAddressRequest();
          jsonRequestBody.ShopperId = eccGetProductsForEmailAddressRequestData.ShopperID;
          jsonRequestBody.ResellerId = eccGetProductsForEmailAddressRequestData.PrivateLabelId.ToString();
          jsonRequestBody.EmailAddress = eccGetProductsForEmailAddressRequestData.EmailAddress;
          jsonRequestBody.Subaccount = eccGetProductsForEmailAddressRequestData.Subaccount;
          
          EccJsonRequest<ECCJsonGetProductsForEmailAddressRequest> jsonRequest = new EccJsonRequest<ECCJsonGetProductsForEmailAddressRequest>
          {
            Id = authName,
            Token = authToken,
            Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, "ar_from", "ASC"),
            Parameters = jsonRequestBody
          };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey);
          responseData = new ECCGetProductsForEmailAddressResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetProductsForEmailAddressResponseData(requestData, ex);
        }
      }

      return responseData;
    }

  }
}
