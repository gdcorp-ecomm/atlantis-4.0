using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeForms.Impl
{
  public static class HttpWebRequestHelper
  {
    public static string SendWebRequest(RequestData requestData , string fullUrl, WsConfigElement configElement)
    {
      string result = string.Empty;

      var webRequest = (HttpWebRequest)WebRequest.Create(fullUrl);
      webRequest.ContentType = "application/x-www-form-urlencoded";
      webRequest.Method = "GET";
      webRequest.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      webRequest.KeepAlive = false;

      if (!string.IsNullOrEmpty(configElement.GetConfigValue("ClientCertificateName")))
      {
        X509Certificate2 clientCertificate = configElement.GetClientCertificate();
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
              result = streamReader.ReadToEnd().Trim();
            }
          }
        }
      }

      return result;
    }
  }
}
