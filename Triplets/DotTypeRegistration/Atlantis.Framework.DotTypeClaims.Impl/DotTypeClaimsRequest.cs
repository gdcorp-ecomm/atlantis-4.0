using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.DotTypeClaims.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeClaims.Impl
{
  public class DotTypeClaimsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeClaimsResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
        var dotTypeClaimsRequestData = (DotTypeClaimsRequestData)requestData;
        var wsConfigElement = ((WsConfigElement)config);

        var fullUrl = string.Format("{0}/api/schema/getclaimsxml?tldid={1}&pl={2}&ph={3}&marketid={4}&domain={5}",
                                    wsConfigElement.WSURL,
                                    dotTypeClaimsRequestData.TldId,
                                    dotTypeClaimsRequestData.Placement,
                                    dotTypeClaimsRequestData.Phase,
                                    dotTypeClaimsRequestData.MarketId,
                                    dotTypeClaimsRequestData.Domain);


        var webRequest = (HttpWebRequest) WebRequest.Create(fullUrl);
        webRequest.ContentType = "application/x-www-form-urlencoded";
        webRequest.Method = "GET";
        webRequest.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
        webRequest.KeepAlive = false;

        if (!string.IsNullOrEmpty(wsConfigElement.GetConfigValue("ClientCertificateName")))
        {
          X509Certificate2 clientCertificate = wsConfigElement.GetClientCertificate();
          webRequest.ClientCertificates.Add(clientCertificate);
        }

        using (var webResponse = webRequest.GetResponse())
        {
          using (var dataStream = webResponse.GetResponseStream())
          {
            if (dataStream != null)
            {
              using (var streamReader = new StreamReader(dataStream))
              {
                responseXml = streamReader.ReadToEnd().Trim();
              }
            }
          }
        }

        responseData = DotTypeClaimsResponseData.FromResponseXml(responseXml);
      }
      catch (WebException ex)
      {
        if (ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
        {
          //no error logging is required
          responseData = DotTypeClaimsResponseData.FromResponseXml(string.Empty);
        }
        else
        {
          var exception = new AtlantisException(requestData, "DotTypeClaimsRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
          responseData = DotTypeClaimsResponseData.FromException(exception);
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException(requestData, "DotTypeClaimsRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        responseData = DotTypeClaimsResponseData.FromException(exception);
      }
      return responseData;
    }
  }
}
