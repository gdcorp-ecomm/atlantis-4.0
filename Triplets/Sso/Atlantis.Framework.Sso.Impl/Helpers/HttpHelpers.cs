using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Script.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Sso.Impl.Helpers
{
  internal static class HttpHelpers
  {
    public static HttpWebRequest GetHttpWebRequestAndAddData(string wsUrl, string urlEncodedData, string contentType, string httpMethod, TimeSpan timeout, string userAgent = null, X509Certificate2 clientCert = null)
    {
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(wsUrl);
      httpWebRequest.Method = httpMethod;
      httpWebRequest.ContentType = contentType;
      httpWebRequest.Timeout = (int)timeout.TotalMilliseconds;

      if (clientCert != null)
      {
        httpWebRequest.ClientCertificates.Add(clientCert);
      }

      if (!string.IsNullOrEmpty(userAgent))
      {
        httpWebRequest.UserAgent = userAgent;
      }

      if (urlEncodedData != null)
      {
        httpWebRequest.ContentLength = urlEncodedData.Length;
        using (var authPostStream = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
          authPostStream.Write(urlEncodedData);
        }
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
        var responseStatusCode = ((HttpWebResponse) exception.Response).StatusCode;
        string errorResponseData = string.Empty;

        using (var errorReader = new StreamReader(exception.Response.GetResponseStream()))
        {
          errorResponseData = errorReader.ReadToEnd();
        }

        if (responseStatusCode == HttpStatusCode.BadRequest)
        {
          returnObject = new JavaScriptSerializer().Deserialize<T>(errorResponseData);
        }
        else
        {
          string errorMessage = string.Concat("Unhandled http status code: ", responseStatusCode.ToString(), " | responseData ", errorResponseData, " | webexception_message:", exception.Message);
          throw new HttpUnhandledException(errorMessage);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }

      return returnObject;
    }
  }
}
