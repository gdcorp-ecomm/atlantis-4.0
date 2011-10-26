using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCSetOFFAccount.Impl.Json;
using Atlantis.Framework.ECCSetOFFAccount.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCSetOFFAccount.Impl
{
  public class ECCSetOFFAccountRequest : IRequest 
  {
    private const string REQUEST_METHOD = "setOFFAccount";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCSetOFFAccountRequestData eccSetOFFAccountRequestData = (ECCSetOFFAccountRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCSetOFFAccountResponseData(requestData, new AuthenticationException("Unable to autheticate ECCGetEmailAddressesForDomainRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          ECCJsonSetOFFAccountRequest jsonRequestBody = new ECCJsonSetOFFAccountRequest();
          jsonRequestBody.ShopperId = eccSetOFFAccountRequestData.ShopperID;
          jsonRequestBody.ResellerId = eccSetOFFAccountRequestData.PrivateLabelId.ToString();
          jsonRequestBody.Username = eccSetOFFAccountRequestData.Username;
          jsonRequestBody.EmailAddress = eccSetOFFAccountRequestData.EmailAddress;
          jsonRequestBody.AccountUid = eccSetOFFAccountRequestData.AccountUid;
          jsonRequestBody.Password = eccSetOFFAccountRequestData.Password;
          
          EccJsonRequest<ECCJsonSetOFFAccountRequest> jsonRequest = new EccJsonRequest<ECCJsonSetOFFAccountRequest>
          {
            Id = authName,
            Token = authToken,
            Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, string.Empty, string.Empty),
            Parameters = jsonRequestBody
          };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey);
          responseData = new ECCSetOFFAccountResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCSetOFFAccountResponseData(requestData, ex);
        }
      }

      return responseData;
    }
  }
}
