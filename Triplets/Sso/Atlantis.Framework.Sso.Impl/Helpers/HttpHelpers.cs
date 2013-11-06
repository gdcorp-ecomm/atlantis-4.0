using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Script.Serialization;

namespace Atlantis.Framework.Sso.Impl.Helpers
{
  internal static class HttpHelpers
  {
    public static HttpWebRequest GetHttpWebRequestAndAddData(string wsUrl, string urlEncodedData, string contentType, string httpMethod, X509Certificate2 clientCert = null)
    {
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(wsUrl);
      httpWebRequest.Method = httpMethod;
      httpWebRequest.ContentType = contentType;

      if (urlEncodedData != null)
      {
        httpWebRequest.ContentLength = urlEncodedData.Length;
        using (var authPostStream = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
          authPostStream.Write(urlEncodedData);
        }
      }

      if (clientCert != null)
      {
        httpWebRequest.ClientCertificates.Add(clientCert);
      }

      return httpWebRequest;
    }

    public static T GetWebResponseAndConvertToObject<T>(HttpWebRequest webRequest)
    {
      T returnObject = default(T);

      try
      {
        using (var webResponse = webRequest.GetResponse() as HttpWebResponse)
        {
          using (var responseReader = new StreamReader(webResponse.GetResponseStream()))
          {
            string tokenWebResponseData = responseReader.ReadToEnd();
            returnObject = new JavaScriptSerializer().Deserialize<T>(tokenWebResponseData);
          }
        }
      }
      catch (WebException exception)
      {
        var responseStatusCode = ((HttpWebResponse)exception.Response).StatusCode;

        if (responseStatusCode == HttpStatusCode.BadRequest)
        {
          using (var errorReader = new StreamReader(exception.Response.GetResponseStream()))
          {
            string errorResponseData = errorReader.ReadToEnd();
            returnObject = new JavaScriptSerializer().Deserialize<T>(errorResponseData);
          }
        }
      }

      return returnObject;
    }
  }
}
