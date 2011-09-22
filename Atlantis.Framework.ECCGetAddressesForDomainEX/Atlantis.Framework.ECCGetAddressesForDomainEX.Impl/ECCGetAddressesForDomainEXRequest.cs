using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetAddressesForDomainEX.Impl.Json;
using Atlantis.Framework.ECCGetAddressesForDomainEX.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetAddressesForDomainEX.Impl
{
  public class ECCGetAddressesForDomainEXRequest : IRequest
  {
    private const string REQUEST_METHOD = "getEmailAddressesForDomain";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetAddressesForDomainEXRequestData eccGetAddressesForDomainExRequestData = (ECCGetAddressesForDomainEXRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetAddressesForDomainEXResponseData(requestData, new AuthenticationException("Unable to authenticate ECCGetAddressesForDomainEXRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          ECCJsonGetAddressesForDomainEXRequest jsonExRequestBody = new ECCJsonGetAddressesForDomainEXRequest();
          jsonExRequestBody.ShopperId = eccGetAddressesForDomainExRequestData.ShopperID;
          jsonExRequestBody.ResellerId = eccGetAddressesForDomainExRequestData.PrivateLabelId.ToString();
          jsonExRequestBody.DomainName = eccGetAddressesForDomainExRequestData.DomainName;
          jsonExRequestBody.EmailType = eccGetAddressesForDomainExRequestData.EmailType.ToString();
          jsonExRequestBody.Active = eccGetAddressesForDomainExRequestData.Active.ToString();
          jsonExRequestBody.SubAccount = eccGetAddressesForDomainExRequestData.SubAccount;

          if (eccGetAddressesForDomainExRequestData.Fields != null)
          {
            jsonExRequestBody.Fields = string.Join(",", eccGetAddressesForDomainExRequestData.Fields.ToArray());
          }
          else
          {
            jsonExRequestBody.Fields = "status,delivery_mode";
          }

          EccJsonRequest<ECCJsonGetAddressesForDomainEXRequest> jsonRequest = new EccJsonRequest<ECCJsonGetAddressesForDomainEXRequest>
          {
            Id = authName,
            Token = authToken,
            Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, string.Empty, string.Empty),
            Parameters = jsonExRequestBody
          };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey, eccGetAddressesForDomainExRequestData.RequestTimeout);
          responseData = new ECCGetAddressesForDomainEXResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetAddressesForDomainEXResponseData(requestData, ex);
        }
      }
      return responseData;
    }

    #endregion
  }
}
