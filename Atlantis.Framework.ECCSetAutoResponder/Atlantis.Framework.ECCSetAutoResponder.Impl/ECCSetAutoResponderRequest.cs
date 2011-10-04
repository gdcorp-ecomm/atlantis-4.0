using System;
using System.Security.Authentication;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCSetAutoResponder.Impl.Json;
using Atlantis.Framework.ECCSetAutoResponder.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCSetAutoResponder.Impl
{
  public class ECCSetAutoResponderRequest : IRequest
  {
    private const string REQUEST_METHOD = "setAutoResponder";
    private const int PAGE_NUMBER = 1;
    private const int RESULTS_PER_PAGE = 100000;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCSetAutoResponderRequestData eccSetAutoResponderRequestData = (ECCSetAutoResponderRequestData)requestData;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);

      string requestKey;
      string authName;
      string authToken;

      if (!NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken))
      {
        responseData = new ECCSetAutoResponderResponseData(requestData,
                                                           new AuthenticationException(
                                                             "Unable to authenticate ECCSetAutoResponderRequest"));
      }
      else
      {
        try
        {
          var wsConfig = ((WsConfigElement)config);

          EccJsonSetAutoResponderRequest jsonRequestBody = new EccJsonSetAutoResponderRequest();
          jsonRequestBody.ShopperId = eccSetAutoResponderRequestData.ShopperID;
          jsonRequestBody.ResellerId = eccSetAutoResponderRequestData.PrivateLabelId.ToString();
          jsonRequestBody.EmailAddress = eccSetAutoResponderRequestData.EmailAddress;
          jsonRequestBody.AutoResponderMessage = eccSetAutoResponderRequestData.AutoResponderMessage;
          jsonRequestBody.AutoResponderSubject = eccSetAutoResponderRequestData.AutoResponderSubject;
          jsonRequestBody.AutoResponderStatus = eccSetAutoResponderRequestData.AutoResponderStatus;
          jsonRequestBody.AutoResponderStart = eccSetAutoResponderRequestData.AutoResponderStart;
          jsonRequestBody.AutoResponderEnd = eccSetAutoResponderRequestData.AutoResponderEnd;

          jsonRequestBody.AutoResponderFrom = eccSetAutoResponderRequestData.AutoResponderFrom;
          jsonRequestBody.SendSingleResponse = eccSetAutoResponderRequestData.SendSingleResponse;
          jsonRequestBody.SubAccount = eccSetAutoResponderRequestData.SubAccount;

          EccJsonRequest<EccJsonSetAutoResponderRequest> jsonRequest = new EccJsonRequest<EccJsonSetAutoResponderRequest>
                                                                         {
                                                                           Id = authName,
                                                                           Token = authToken,
                                                                           Return = new EccJsonPaging(PAGE_NUMBER, RESULTS_PER_PAGE, string.Empty, "ASC"),
                                                                           Parameters = jsonRequestBody
                                                                         };

          string sRequest = jsonRequest.ToJson();
          string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, REQUEST_METHOD, requestKey);
          responseData = new ECCSetAutoResponderResponseData(response);
        }
        catch (Exception ex)
        {
          responseData = new ECCSetAutoResponderResponseData(requestData, ex);
        }
      }

      return responseData;
    }
  }
}
