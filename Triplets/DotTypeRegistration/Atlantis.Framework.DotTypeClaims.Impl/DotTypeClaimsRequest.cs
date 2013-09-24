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

        var fullUrl = wsConfigElement.WSURL + "/getclaimdata?d=" + string.Join(",", dotTypeClaimsRequestData.Domains);

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
      catch (Exception ex)
      {
        var exception = new AtlantisException(requestData, "DotTypeClaimsRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        responseData = DotTypeClaimsResponseData.FromException(exception);
      }

      return responseData;
    }
  }
}
