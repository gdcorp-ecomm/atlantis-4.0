using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace Atlantis.Framework.Sso.Impl.Helpers
{
  internal static class HttpHelpers
  {
    public static HttpWebRequest GetHttpWebRequestAndAddByteData(string wsUrl, byte[] byteData, string contentType, string httpMethod)
    {
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(wsUrl);
      httpWebRequest.Method = httpMethod;
      httpWebRequest.ContentType = contentType;

      if (byteData != null)
      {
        httpWebRequest.ContentLength = byteData.Length;
        using (Stream authPostStream = httpWebRequest.GetRequestStream())
        {
          authPostStream.Write(byteData, 0, byteData.Length);
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
