using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetEmailAddressesForDomain.Impl.Json;
using Atlantis.Framework.ECCGetEmailAddressesForDomain.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetEmailAddressesForDomain.Impl
{
  public class ECCGetEmailAddressesForDomainRequest : IRequest
  {
    private const string REQUEST_METHOD = "getEmailAddressesForDomain";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetEmailAddressesForDomainRequestData eccGetEmailAddressesForDomainRequestData = (ECCGetEmailAddressesForDomainRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetEmailAddressesForDomainResponseData(requestData, new AuthenticationException("Unable to autheticate ECCGetEmailAddressesForDomainRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          ECCJsonGetEmailAddressesForDomainRequest jsonRequestBody = new ECCJsonGetEmailAddressesForDomainRequest();
          jsonRequestBody.ShopperId = eccGetEmailAddressesForDomainRequestData.ShopperID;
          jsonRequestBody.ResellerId = eccGetEmailAddressesForDomainRequestData.PrivateLabelId.ToString();
          jsonRequestBody.DomainName = eccGetEmailAddressesForDomainRequestData.DomainName;
          jsonRequestBody.AccountType = eccGetEmailAddressesForDomainRequestData.AccountType;
          jsonRequestBody.ActiveOnly = eccGetEmailAddressesForDomainRequestData.ActiveOnly;

          EccJsonRequest<ECCJsonGetEmailAddressesForDomainRequest> jsonRequest = new EccJsonRequest<ECCJsonGetEmailAddressesForDomainRequest>
          {
            Id = authName,
            Token = authToken,
            Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, "ar_from", "ASC"),
            Parameters = jsonRequestBody
          };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey);
          responseData = new ECCGetEmailAddressesForDomainResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetEmailAddressesForDomainResponseData(requestData, ex);
        }
      }

      return responseData;
    }
  }
}