using System;
using Atlantis.Framework.ECCGetEmailPodDetails.Interface;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCGetEmailPodDetails.Impl
{
  public class ECCGetEmailPodDetailsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ECCGetEmailPodDetailsRequestData eccGetEmailPodDetailsRequestData = (ECCGetEmailPodDetailsRequestData)requestData;

      const string requestMethod = "getEmailAuthRec";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        var wsConfig = (WsConfigElement)config;

        EccJsonGetEmailPodDetailsRequst jsonRequestBody = new EccJsonGetEmailPodDetailsRequst();
        jsonRequestBody.EmailAddress = eccGetEmailPodDetailsRequestData.EmailAddress;

        EccJsonRequest<EccJsonGetEmailPodDetailsRequst> eccJsonRequest = new EccJsonRequest<EccJsonGetEmailPodDetailsRequst> { Id = authName,
                                                                                                                               Token = authToken,
                                                                                                                               Parameters = jsonRequestBody };

        string sRequest = eccJsonRequest.ToJson();
        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new ECCGetEmailPodDetailsResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new ECCGetEmailPodDetailsResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
