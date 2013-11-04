using Atlantis.Framework.Interface;
using Atlantis.Framework.TargetedShopperService.Interface;
using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace Atlantis.Framework.TargetedShopperService.Impl
{
  public class DecodeXid : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ShopperXidDecodeResponseData responseData = null;
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
        responseData = new ShopperXidDecodeResponseData(output);
      }
      catch (Exception ex)
      {
        responseData = new ShopperXidDecodeResponseData(requestData, ex);
      }

      return responseData;
    }

    private WebRequest GetWebRequest(WsConfigElement config, RequestData oRequestData)
    {
      ShopperXidDecodeRequestData requestData = (ShopperXidDecodeRequestData)oRequestData;


      HttpWebRequest result;
      UriBuilder urlBuilder = new UriBuilder(BuildRequestUrl(config.WSURL, requestData.EncodedXid));

      Uri uri = urlBuilder.Uri;
      result = (HttpWebRequest)WebRequest.Create(uri);
      result.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      result.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
      result.ClientCertificates.Add(config.GetClientCertificate("CertificateName"));
      result.Accept = "text/xml";
      return result;
    }

    private string BuildRequestUrl(string webServiceUrl, string encodedString)
    {
      return String.Format("{0}/{1}", webServiceUrl, encodedString);
    }
  }
}
