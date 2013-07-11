using Atlantis.Framework.Interface;
using Atlantis.Framework.TargetedShopperService.Interface;
using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace Atlantis.Framework.TargetedShopperService.Impl
{
  public class EncodeXid : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ShopperXidEncodeResponseData responseData = null;
      WsConfigElement serviceConfig = (WsConfigElement)config;
      WebRequest request = GetWebRequest(serviceConfig, requestData);

      try
      {
        string output;
        using (WebResponse response = request.GetResponse())
        {
          using (Stream receiveStream = response.GetResponseStream())
          {
            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
            {
              output = readStream.ReadToEnd();
            }
          }
        }
        responseData = new ShopperXidEncodeResponseData(output);
      }
      catch (Exception ex)
      {
        responseData = new ShopperXidEncodeResponseData(requestData, ex);
      }

      return responseData;
    }

    private WebRequest GetWebRequest(WsConfigElement config, RequestData oRequestData)
    {
      ShopperXidEncodeRequestData requestData = (ShopperXidEncodeRequestData)oRequestData;


      HttpWebRequest result;
      UriBuilder urlBuilder = new UriBuilder(BuildRequestUrl(config.WSURL, requestData.DecodedXid));

      Uri uri = urlBuilder.Uri;
      result = (HttpWebRequest)WebRequest.Create(uri);
      result.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      result.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
      result.Accept = "text/xml";
      return result;
    }

    private string BuildRequestUrl(string webServiceUrl, string xid)
    {
      return String.Format("{0}/{1}", webServiceUrl, xid);
    }
  }
}
