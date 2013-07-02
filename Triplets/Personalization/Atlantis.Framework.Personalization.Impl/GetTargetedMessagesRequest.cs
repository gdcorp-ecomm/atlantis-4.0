using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Personalization.Impl
{
  public class GetTargetedMessagesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      TargetedMessagesResponseData responseData = null;
      WsConfigElement serviceConfig = (WsConfigElement)oConfig;
      WebRequest request = GetWebRequest(serviceConfig, oRequestData);

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
        responseData = new TargetedMessagesResponseData(output);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new TargetedMessagesResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new TargetedMessagesResponseData(oRequestData, ex);
      }
      return responseData;
    }



    private WebRequest GetWebRequest(WsConfigElement config, RequestData oRequestData)
    {
      TargetedMessagesRequestData requestData = (TargetedMessagesRequestData)oRequestData;

      string appID = requestData.AppId;
      string interactionPoint = requestData.InteractionPoint;

      HttpWebRequest result;
      UriBuilder urlBuilder = new UriBuilder(BuildRequestUrl(config.WSURL, appID, interactionPoint));

      string query = BuildRequestQuery(requestData);

      if (!string.IsNullOrEmpty(query))
      {
        urlBuilder.Query = query;
      }

      Uri uri = urlBuilder.Uri;
      result = (HttpWebRequest)WebRequest.Create(uri);
      result.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      result.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
      result.Accept = "text/xml";
      return result;
    }

    private string BuildRequestUrl(string webServiceUrl, string appID, string interactionPoint)
    {
      return String.Format("{0}/{1}/{2}", webServiceUrl, appID, interactionPoint);
    }

    private string BuildRequestQuery(TargetedMessagesRequestData requestData)
    {
      List<string> queryItems = new List<string>();

      if (!String.IsNullOrEmpty(requestData.ContextData))
      {
        queryItems.Add(String.Format("{0}={1}", "contextData", Uri.EscapeDataString(requestData.ContextData)));
      }
      if (!String.IsNullOrEmpty(requestData.ShopperData))
      {
        queryItems.Add(String.Format("{0}={1}", "shopperData", Uri.EscapeDataString(requestData.ShopperData)));
      }

      queryItems.Sort();

      return string.Join("&", queryItems);
    }

  }
}