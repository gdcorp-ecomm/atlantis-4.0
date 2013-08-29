using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace Atlantis.Framework.Products.Impl
{
  public class ProductNamesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var productNamesRequest = (ProductNamesRequestData)requestData;

      if (string.IsNullOrEmpty(productNamesRequest.FullLanguage))
      {
        return ProductNamesResponseData.Empty;
      }

      var url = ((WsConfigElement)config).WSURL + productNamesRequest.NonUnifiedPfid.ToString(CultureInfo.InvariantCulture) + "/" + productNamesRequest.FullLanguage;
      var xml = GetServiceDataXml(url, requestData.RequestTimeout);
      return ProductNamesResponseData.FromServiceData(xml);
    }

    private string GetServiceDataXml(string url, TimeSpan timeout)
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
