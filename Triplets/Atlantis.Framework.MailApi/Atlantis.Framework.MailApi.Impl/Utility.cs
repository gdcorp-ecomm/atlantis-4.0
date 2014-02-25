using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Atlantis.Framework.MailApi.Impl
{
  class Utility
  {
    private const string COOKIE_STRING = "{{\"session\":\"{0}\",\"app_key\":\"{1}\"}}";
    private const string COOKIE_STRING_RESTRICTED = "{{\"session\":\"{0}\",\"app_key\":\"{1}\",\"key\":\"{2}\"}}";
    private const string DefaultBaseUrl = "mailapi.secureserver.net";

    public static string PostRequest(string url, string messageBody, string session, string appKey, string key)
    {
      string jsonResponse = null;

      var request = (HttpWebRequest)WebRequest.Create(url);
      request.Method = WebRequestMethods.Http.Post;

      byte[] bodyBytes = Encoding.UTF8.GetBytes(messageBody);

      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = bodyBytes.Length;

      if (!string.IsNullOrEmpty(session))
      {
        AddStateCookieToRequest(request, url, session, appKey, key);
      }

      using (var requestStream = request.GetRequestStream())
      {
        requestStream.Write(bodyBytes, 0, bodyBytes.Length);
        requestStream.Close();
      }

      var response = (HttpWebResponse)request.GetResponse();

      using (var responseStream = response.GetResponseStream())
      {
        if (responseStream != null)
        {
          using (var responseReader = new StreamReader(responseStream, Encoding.UTF8))
          {
            jsonResponse = responseReader.ReadToEnd();
            responseReader.Close();
          }
          responseStream.Close();
        }
      }

      return jsonResponse;
    }

    public static string BuildWebServiceUrl(string mailBaseUrl, string wsUrl)
    {
      string webServiceUrl = mailBaseUrl.Contains("80") ? "http://" : "https://";
      webServiceUrl += !string.IsNullOrEmpty(mailBaseUrl) ? mailBaseUrl : DefaultBaseUrl;
      webServiceUrl += wsUrl;
      return webServiceUrl;
    }

    private static void AddStateCookieToRequest(HttpWebRequest request, string url, string mailHash, string appkey, string key)
    {
      string encodedCookieValue = string.IsNullOrEmpty(key) ? HttpUtility.UrlEncode(String.Format(COOKIE_STRING, mailHash, appkey)) : HttpUtility.UrlEncode(String.Format(COOKIE_STRING_RESTRICTED, mailHash, appkey, key));
      Cookie sessionCookie = new Cookie("state", encodedCookieValue);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(url), sessionCookie);
    }
  }
}
