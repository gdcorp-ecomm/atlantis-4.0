using Atlantis.Framework.BazaarVoiceAPI.Interface;
using Atlantis.Framework.Interface;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Atlantis.Framework.BazaarVoiceAPI.Impl
{
  public class ReviewsSEORequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;


      WsConfigElement serviceConfig = (WsConfigElement)config;

      WebRequest request = GetWebRequest(serviceConfig, requestData);

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

      string responseHtml = ParseResponse(serviceConfig, output);

      result = new ReviewsSEOResponseData(responseHtml);


      return result;

    }

    private string ParseResponse(WsConfigElement config, string output)
    {
      string separator = config.WSURL.Contains("?") ? "&" : "?";
      string url = config.WSURL.EndsWith("/") ? config.WSURL.Substring(config.WSURL.Length - 1) : config.WSURL;

      string result = output.Replace("{INSERT_PAGE_URI}", string.Format("{0}{1}", url, separator));

      return result;

    }

    private WebRequest GetWebRequest(WsConfigElement config, RequestData requestData)
    {
      WebRequest result;
      ReviewsSEORequestData apiRequestData = (ReviewsSEORequestData)requestData;
      string url = config.WSURL.EndsWith("/") ? config.WSURL : string.Format("{0}/", config.WSURL);

      string apiKey = string.IsNullOrEmpty(apiRequestData.APIKey) ? config.GetConfigValue("APIKey") : apiRequestData.APIKey;
      string DisplayCode = string.IsNullOrEmpty(apiRequestData.DisplayCode) ? config.GetConfigValue("DisplayCode") : apiRequestData.DisplayCode;
      string fullPath = string.Format("{0}{1}/{2}/reviews/product/{3}/{4}.htm", url, apiKey, DisplayCode, apiRequestData.PageNumber.ToString(), apiRequestData.BVProductId);

      UriBuilder urlBuilder = new UriBuilder(fullPath);
      Uri uri = urlBuilder.Uri;
      result = HttpWebRequest.Create(uri);
      result.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      //result.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);  TODO:  Check with Micco if I need this
      return result;
    }
  }
}
