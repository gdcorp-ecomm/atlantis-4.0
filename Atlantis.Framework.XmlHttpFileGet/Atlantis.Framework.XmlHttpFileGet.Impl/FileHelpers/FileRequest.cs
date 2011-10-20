using System;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace Atlantis.Framework.XmlHttpFileGet.Impl
{
  internal static class FileRequest
  {
    public static string SendRequest(string requestUrl, TimeSpan requestTimeout, RequestCacheLevel cacheLevel)
    {
      var response = string.Empty;
      var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
      if (webRequest != null)
      {
        webRequest.Timeout = (int) requestTimeout.TotalMilliseconds;
        webRequest.Method = "GET";
        webRequest.ContentType = "text/xml; encoding='utf-8'";
        webRequest.CachePolicy = new RequestCachePolicy(cacheLevel);
        
        var webResponse = webRequest.GetResponse() as HttpWebResponse;
        if (webResponse != null)
        {
          using (Stream webResponseData = webResponse.GetResponseStream())
          {
            if (webResponseData != null)
            {
              using (StreamReader responseReader = new StreamReader(webResponseData))
              {
                response = responseReader.ReadToEnd();
                responseReader.Close();
              }
            }
          }
        }
      }
      return response;
    }
  }
}