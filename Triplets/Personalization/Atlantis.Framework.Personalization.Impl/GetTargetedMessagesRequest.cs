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
    private readonly Lazy<bool> _canCallTMS = new Lazy<bool>(() => DataCache.DataCache.GetAppSetting("ATLANTIS_PERSONALIZATION_TRIPLET_TMS_ON").Equals("true", StringComparison.OrdinalIgnoreCase));

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      TargetedMessagesResponseData responseData = null;
      if (_canCallTMS.Value)
      {
        TargetedMessagesRequestData requestData = (TargetedMessagesRequestData)oRequestData;

        WsConfigElement serviceConfig = (WsConfigElement)oConfig;
        string url;

        if (serviceConfig.WSURL.EndsWith("/"))
        {
          url = string.Concat(serviceConfig.WSURL, requestData.GetWebServicePath());
        }
        else
        {
          url = string.Concat(serviceConfig.WSURL, "/", requestData.GetWebServicePath());
        }

        Stream dataStream = null;

        try
        {
          string postData = requestData.GetPostData();
          var buffer = Encoding.UTF8.GetBytes(postData);

          HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create((new UriBuilder(url)).Uri);

          if (webRequest != null)
          {
            webRequest.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
            webRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/xml";
            webRequest.Accept = "application/xml";
            webRequest.ContentLength = buffer.Length;
            webRequest.KeepAlive = false;
            dataStream = webRequest.GetRequestStream();
          }

          if (dataStream != null)
          {
            dataStream.Write(buffer, 0, buffer.Length);
            dataStream.Close();
          }

          string response = null;
          if (webRequest != null)
          {
            var webResponse = webRequest.GetResponse() as HttpWebResponse;

            if (webResponse != null)
            {
              var webResponseData = webResponse.GetResponseStream();
              if (webResponseData != null)
              {
                StreamReader responseReader = null;
                try
                {
                  responseReader = new StreamReader(webResponseData);
                  response = responseReader.ReadToEnd();
                }
                finally
                {
                  if (responseReader != null)
                  {
                    responseReader.Dispose();
                  }
                }
              }
            }
          }

          responseData = new TargetedMessagesResponseData(response, url);
        }
        catch (Exception ex)
        {
          responseData = new TargetedMessagesResponseData(requestData, ex, url);
        }
        finally
        {
          if (dataStream != null)
          {
            dataStream.Dispose();
          }
        }
      }
      else
      {
        responseData = new TargetedMessagesResponseData(true);
      }

      return responseData;
    }

  }
}