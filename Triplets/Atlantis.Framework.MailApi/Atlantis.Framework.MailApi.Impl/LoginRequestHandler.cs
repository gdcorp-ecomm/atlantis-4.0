using System;
using System.IO;
using System.Net;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;

namespace Atlantis.Framework.MailApi.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.MailApi.Impl.LoginRequestHandler" assembly="Atlantis.Framework.MailApi.Impl.dll" request_type="###" />
  
  public class LoginRequestHandler : IRequest
  {
    private const string BodyString = "method=login&params={{\"username\":\"{0}\",\"password\":\"{1}\"}}&state={{\"app_key\":\"{2}\"}}";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (LoginRequestData)requestData;
      string username = System.Web.HttpUtility.UrlEncode(request.Username); // Merc #159866
      string password = System.Web.HttpUtility.UrlEncode(request.Password); // this is ALREADY escaped in GDAndroid code
      string appKey = request.Appkey;

      string webServiceUrl = ((WsConfigElement) config).WSURL;

      string messageBody = String.Format(BodyString, username, password, appKey);

      LoginResponseData loginResponse = PostRequest(webServiceUrl, messageBody);

      return loginResponse;
    }

    private LoginResponseData PostRequest(string url, string messageBody)
    {
      string jsonResponse = null;

      HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
      request.Method = WebRequestMethods.Http.Post;

      byte[] bodyBytes = Encoding.UTF8.GetBytes(messageBody);

      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = bodyBytes.Length;

      Stream requestStream = request.GetRequestStream();
      requestStream.Write(bodyBytes, 0, bodyBytes.Length);
      requestStream.Close();

      HttpWebResponse loginResponse = (HttpWebResponse) request.GetResponse();
      Stream responseStream = loginResponse.GetResponseStream();
      StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8);
      jsonResponse = responseReader.ReadToEnd();

      responseStream.Close();
      responseReader.Close();

      return LoginResponseData.FromJsonData(jsonResponse);
    }
  }
}
