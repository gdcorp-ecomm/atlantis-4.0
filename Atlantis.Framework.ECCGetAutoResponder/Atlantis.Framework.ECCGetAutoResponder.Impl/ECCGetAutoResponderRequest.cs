using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCGetAutoResponder.Impl.Json;
using Atlantis.Framework.ECCGetAutoResponder.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetAutoResponder.Impl
{
  public class ECCGetAutoResponderRequest : IRequest
  {
    private const string REQUEST_METHOD = "getAutoResponderForEmail";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetAutoResponderRequestData eccGetAutoResponderRequestData = (ECCGetAutoResponderRequestData) requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if(!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCGetAutoResponderResponseData(requestData, new AuthenticationException("Unable to autheticate ECCGetAutoResponderRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          EccJsonGetAutoResponderRequest jsonRequestBody = new EccJsonGetAutoResponderRequest();
          jsonRequestBody.ShopperId = eccGetAutoResponderRequestData.ShopperID;
          jsonRequestBody.ResellerId = eccGetAutoResponderRequestData.PrivateLabelId.ToString();
          jsonRequestBody.EmailAddress = eccGetAutoResponderRequestData.EmailAddress;
          jsonRequestBody.SubAccount = eccGetAutoResponderRequestData.SubAccount;

          EccJsonRequest<EccJsonGetAutoResponderRequest> jsonRequest = new EccJsonRequest<EccJsonGetAutoResponderRequest>
          {
            Id = authName,
            Token = authToken,
            Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, "ar_from", "ASC"),
            Parameters = jsonRequestBody
          };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey);
          responseData = new ECCGetAutoResponderResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCGetAutoResponderResponseData(requestData, ex);
        }
      }

      return responseData;
    }
  }
}
