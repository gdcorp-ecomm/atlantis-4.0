using System.IO;
using System.Net;
using System.Net.Cache;

namespace Atlantis.Framework.CDS.Impl
{
  internal class CDSService
  {
    public string Url { get; set; }
    
    public CDSService(string url)
    {
      Url = url;
    }

    public string GetWebResponse()
    {
      string responseText = string.Empty;

      HttpWebResponse webResponse = null;
      WebRequest webRequest = WebRequest.Create(Url) as HttpWebRequest;

      if (webRequest != null)
      {
        webRequest.Method = "GET";
        webRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
        webResponse = webRequest.GetResponse() as HttpWebResponse;

        if (webResponse != null)
        {
          if (webResponse.StatusCode == HttpStatusCode.OK)
          {
            using (Stream webResponseData = webResponse.GetResponseStream())
            {
              if (webResponseData != null)
              {
                using (StreamReader responseReader = new StreamReader(webResponseData))
                {
                  responseText = responseReader.ReadToEnd();
                  responseReader.Close();
                }
              }
            }
          }
        }
      }

      return responseText;
    }

  }
}
