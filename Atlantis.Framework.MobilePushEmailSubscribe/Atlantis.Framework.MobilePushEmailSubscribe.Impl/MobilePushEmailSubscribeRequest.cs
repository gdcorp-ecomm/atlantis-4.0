using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobilePushEmailSubscribe.Interface;

namespace Atlantis.Framework.MobilePushEmailSubscribe.Impl
{
  public class MobilePushEmailSubscribeRequest : IRequest
  {
    private static void PostRequest(string requestUrl, TimeSpan requestTimeout, IEnumerable<KeyValuePair<string, string>> requestHeaders, out IDictionary<string, string> responseHeaders)
    {
      responseHeaders = new Dictionary<string, string>(16);

      try
      {
        HttpWebRequest webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;

        if (webRequest != null)
        {
          webRequest.Timeout = (int)requestTimeout.TotalMilliseconds;
          webRequest.Method = "POST";
          webRequest.ContentType = "application/x-www-form-urlencoded";
          webRequest.ContentLength = 0;

          foreach (KeyValuePair<string, string> customRequestHeader in requestHeaders)
          {
            webRequest.Headers.Add(customRequestHeader.Key, customRequestHeader.Value);
          }

        }

        if (webRequest != null)
        {
          using (HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse)
          {
            if (webResponse != null && webRequest.HaveResponse)
            {
              using (Stream webResponseData = webResponse.GetResponseStream())
              {
                if (webResponseData != null)
                {
                  foreach (string headerKey in webResponse.Headers.AllKeys)
                  {
                    responseHeaders[headerKey] = webResponse.Headers[headerKey];
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error posting to ECC subscribe service. {0}", ex.Message));
      }
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      MobilePushEmailSubscribeRequestData mobilePushEmailSubscribeRequestData = (MobilePushEmailSubscribeRequestData)requestData;
      WsConfigElement wsConfigElement = (WsConfigElement) config;

      try
      {
        string callBackBaseUrl = wsConfigElement.GetConfigValue("NotificationServiceUrl");
        if (string.IsNullOrEmpty(callBackBaseUrl))
        {
          throw new Exception("NotificationServiceUrl must be set in the atlantis.config as a config value");
        }

        if(string.IsNullOrEmpty(mobilePushEmailSubscribeRequestData.PushRegistrationId))
        {
          throw new Exception("PushRegistrationId cannot be empty.");
        }

        if (string.IsNullOrEmpty(mobilePushEmailSubscribeRequestData.Email))
        {
          throw new Exception("Email cannot be empty.");
        }

        string callBackUrl = string.Format("{0}?action=Notification&login={1}&registrationId={2}", callBackBaseUrl,
                                                                                                   mobilePushEmailSubscribeRequestData.Email,
                                                                                                   mobilePushEmailSubscribeRequestData.PushRegistrationId);

        string subscriptionUrl = string.Format("{0}?action=SUBSCRIBE&login={1}", wsConfigElement.WSURL, mobilePushEmailSubscribeRequestData.Email);

        ICollection<KeyValuePair<string, string>> requestHeaders = new Collection<KeyValuePair<string, string>>();
        requestHeaders.Add(new KeyValuePair<string, string>("X-Call-Back", callBackUrl));
        requestHeaders.Add(new KeyValuePair<string, string>("X-Notification-Info", string.Empty));

        IDictionary<string, string> responseHeaders;
        PostRequest(subscriptionUrl, requestData.RequestTimeout, requestHeaders, out responseHeaders);
        
        string subscriptionIdHeaderValue;
        long subscriptionId;
        if (responseHeaders.TryGetValue("X-Subscription-Id", out subscriptionIdHeaderValue) &&
            long.TryParse(subscriptionIdHeaderValue, out subscriptionId))
        {
          responseData = new MobilePushEmailSubscribeResponseData(subscriptionId);
        }
        else
        {
          StringBuilder responseHeadersBuilder = new StringBuilder();
          foreach (KeyValuePair<string, string> responseHeader in responseHeaders)
          {
            responseHeadersBuilder.AppendFormat("{0}: {1},", responseHeader.Key, responseHeader.Value);
          }
          
          throw new Exception(string.Format("SubscriptionId not returned. ResponseHeaders: {0}", responseHeadersBuilder));
        }
      }
      catch(Exception ex)
      {
        responseData = new MobilePushEmailSubscribeResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
