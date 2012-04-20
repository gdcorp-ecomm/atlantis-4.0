using System;
using System.Xml.Linq;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GoogleGetToken.Interface;
using System.Web.Script.Serialization;
using Atlantis.Framework.Nimitz;
using System.Web;
using Atlantis.Framework.Google;

namespace Atlantis.Framework.GoogleGetToken.Impl
{
  public class GoogleRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GoogleGetTokenResponseData responseData;
      var tokenRequestData = requestData as GoogleGetTokenRequestData;
      try
      {
        if (tokenRequestData == null)
        {
          throw new Exception("GoogleGetTokenRequestData requestData is null");
        }

        string uri = ((WsConfigElement)config).WSURL; // "https://accounts.google.com/o/oauth2/token";
        string body = "code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code";

        string connectionstring = NetConnect.LookupConnectInfo(config,ConnectLookupType.Xml);

        XDocument authXml = XDocument.Parse(connectionstring);
        string client_id = (from curNode in authXml.Descendants("Connect")
                            select curNode.Element("UserID").Value).First();

        string password = (from curNode in authXml.Descendants("Connect")
                           select curNode.Element("Password").Value).First();


        string postBody = string.Format(body, tokenRequestData.oAuthToken, client_id, password, HttpUtility.UrlEncode(tokenRequestData.UriRedirect));

        var response = DataRequest.PostRequest(uri, postBody, "application/x-www-form-urlencoded", tokenRequestData.RequestTimeout, tokenRequestData.CacheLevel);
        if (!string.IsNullOrEmpty(response))
        {
          JavaScriptSerializer serializer = new JavaScriptSerializer();
          responseData = new GoogleGetTokenResponseData((auth)serializer.Deserialize<auth>(response), DateTime.Now);
        }
        else
        {
          var aex = new AtlantisException(
            requestData, "GoogleGetToken.RequestHandler",
            "Request returned an empty xml.", tokenRequestData.oAuthToken);
          responseData = new GoogleGetTokenResponseData(aex);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        string data = tokenRequestData.oAuthToken;
        var aex = new AtlantisException(requestData, "GoogleGetToken.RequestHandler", message, data);
        responseData = new GoogleGetTokenResponseData(aex);
      }

      return responseData;
    }
  }
}