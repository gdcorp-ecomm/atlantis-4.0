using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GoogleDNSInsert.Interface;
using System.Web.Script.Serialization;
using Atlantis.Framework.Nimitz;
using System.Web;
using Atlantis.Framework.Google;
using System.Collections.Specialized;
using System.Net;

namespace Atlantis.Framework.GoogleDNSInsert.Impl
{
  public class GoogleRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GoogleDNSInsertResponseData responseData;
      var tokenRequestData = requestData as GoogleDNSInsertRequestData;
      try
      {
        if (tokenRequestData == null)
        {
          throw new Exception("GoogleDNSInsertRequestData requestData is null");
        }

        string ws_url = ((WsConfigElement)config).WSURL;

        string full_url = string.Concat(ws_url, "?verificationMethod=DNS");   

        webresource resource = new webresource
        {
          id = null,
          site = new site
          {
            identifier = tokenRequestData.DomainName,
            type = "INET_DOMAIN"
          },
          owners = new string[1]
        };

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string postBody = serializer.Serialize(resource);

        NameValueCollection header = new NameValueCollection(1);
        header.Add("Authorization", string.Format("OAuth {0}", tokenRequestData.PrivateAccessToken));

        bool isSuccess;
        string response = DataRequest.PostRequest(full_url, postBody, "application/json", header, tokenRequestData.RequestTimeout, tokenRequestData.CacheLevel, out isSuccess);

        responseData = new GoogleDNSInsertResponseData(response, isSuccess, DateTime.Now);
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        string data = tokenRequestData.DomainName;
        var aex = new AtlantisException(requestData, "GoogleDNSInsert.RequestHandler", message, data);
        responseData = new GoogleDNSInsertResponseData(aex);
        
      }

      return responseData;
    }
  }
}