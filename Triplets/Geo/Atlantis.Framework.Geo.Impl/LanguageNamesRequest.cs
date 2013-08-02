using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Geo.Impl
{
  public abstract class LanguageNamesRequest : IRequest
  {
    public abstract IResponseData RequestHandler(RequestData requestData, ConfigElement config);

    protected string GetServiceDataXml(string url, TimeSpan timeout)
    {
      var restCall = WebRequest.Create(url);
      restCall.Timeout = (int)timeout.TotalMilliseconds;
      restCall.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.BypassCache);

      var response = restCall.GetResponse();
      string xml;

      try
      {
        using (var stream = response.GetResponseStream())
        {
          if (stream == null)
          {
            throw new Exception("Response stream null: " + url);
          }

          using (var reader = new StreamReader(stream))
          {
            xml = reader.ReadToEnd();
          }
        }
      }
      finally
      {
        response.Close();
      }

      return xml;
    }
  }
}
