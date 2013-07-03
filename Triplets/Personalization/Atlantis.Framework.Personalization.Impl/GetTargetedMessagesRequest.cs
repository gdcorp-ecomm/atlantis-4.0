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
      string shopperId = requestData.ShopperID;
      string privateLabel = requestData.PrivateLabel;

      HttpWebRequest result;
      UriBuilder urlBuilder = new UriBuilder(BuildRequestUrl(config.WSURL, appID, interactionPoint, shopperId, privateLabel));

      Uri uri = urlBuilder.Uri;
      result = (HttpWebRequest)WebRequest.Create(uri);
      result.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      result.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
      result.Accept = "text/xml";
      return result;
    }

    private string BuildRequestUrl(string webServiceUrl, string appID, string interactionPoint, string shopperId, string privateLabel)
    {
      return String.Format("{0}/{1}/{2}?shopperData=shopperID={3}|privateLabelID={4}", webServiceUrl, appID, interactionPoint, shopperId, privateLabel);
    }
  }
}