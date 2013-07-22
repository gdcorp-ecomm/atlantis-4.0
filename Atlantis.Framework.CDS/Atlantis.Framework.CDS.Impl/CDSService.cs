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

      WebRequest webRequest = WebRequest.Create(Url);

      webRequest.Method = "GET";
      webRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
      
      using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
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

      return responseText;
    }

  }
}
