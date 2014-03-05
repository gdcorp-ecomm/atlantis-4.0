using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;
using System;

namespace Atlantis.Framework.MailApi.Impl
{
  public class LoginRequestHandler : IRequest
  {
    private const string BodyString = "method=login&params={{\"username\":\"{0}\",\"password\":\"{1}\"}}&state={{\"app_key\":\"{2}\"}}";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (LoginRequestData)requestData;
      string username = System.Web.HttpUtility.UrlEncode(request.Username); // Merc #159866
      string password = System.Web.HttpUtility.UrlEncode(request.Password); // this is ALREADY escaped in GDAndroid code
      string appKey = request.Appkey;

      string webServiceUrl = ((WsConfigElement)config).WSURL;

      string messageBody = String.Format(BodyString, username, password, appKey);

      string loginResponseString = Utility.PostRequest(webServiceUrl, messageBody, null, null, null);

      LoginResponseData loginResponse = LoginResponseData.FromJsonData(loginResponseString);

      loginResponse.request = request;

      return loginResponse;
    }
  }
}
