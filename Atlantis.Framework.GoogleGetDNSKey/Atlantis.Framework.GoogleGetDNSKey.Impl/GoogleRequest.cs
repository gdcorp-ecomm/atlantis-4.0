using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GoogleGetDNSKey.Interface;
using System.Web.Script.Serialization;
using System.Web;
using Atlantis.Framework.Google;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.GoogleGetDNSKey.Impl
{
  public class GoogleRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GoogleGetDNSKeyResponseData responseData;
      var tokenRequestData = requestData as GoogleGetDNSKeyRequestData;
      string responseString=string.Empty;
      try
      {
        if (tokenRequestData == null)
        {
          throw new Exception("GoogleGetDNSKeyRequestData requestData is null");
        }

        string ws_url = ((WsConfigElement)config).WSURL;

        string full_uri = string.Concat(ws_url, "?access_token=", tokenRequestData.PrivateAccessToken);

        requestBody body = new requestBody
        {
          verificationMethod = "DNS",
          site = new site
          {
            identifier = tokenRequestData.DomainName,
            type = "INET_DOMAIN"
          }
        };

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string postBody = serializer.Serialize(body);

        responseString = DataRequest.PostRequest(full_uri, postBody, "application/json", tokenRequestData.RequestTimeout, tokenRequestData.CacheLevel);
        if (!string.IsNullOrEmpty(responseString))
        {
          responseData = new GoogleGetDNSKeyResponseData((verification)serializer.Deserialize<verification>(responseString), DateTime.Now);
        }
        else
        {
          var aex = new AtlantisException(
            requestData, "GoogleGetDNSKey.RequestHandler",
            "Request returned an empty xml.", tokenRequestData.PrivateAccessToken);
          responseData = new GoogleGetDNSKeyResponseData(aex);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        string data = tokenRequestData.PrivateAccessToken;
        var aex = new AtlantisException(requestData, "GoogleGetDNSKey.RequestHandler", message, responseString);
        responseData = new GoogleGetDNSKeyResponseData(aex);
      }

      return responseData;
    }
  }
}