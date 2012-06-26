using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobilePushEmailUnsubscribe.Interface;

namespace Atlantis.Framework.MobilePushEmailUnsubscribe.Impl
{
  public class MobilePushEmailUnsubscribeRequest : IRequest
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

      MobilePushEmailUnsubscribeRequestData mobilePushEmailUnsubscribeRequestData = (MobilePushEmailUnsubscribeRequestData)requestData;
      WsConfigElement wsConfigElement = (WsConfigElement)config;

      try
      {
        if (mobilePushEmailUnsubscribeRequestData.SubscriptionId <= 0)
        {
          throw new Exception("SubscriptionId must be greater than zero.");
        }

        if (string.IsNullOrEmpty(mobilePushEmailUnsubscribeRequestData.Email))
        {
          throw new Exception("Email cannot be empty.");
        }

        string unsubscribeUrl = string.Format("{0}?action=UNSUBSCRIBE&login={1}", wsConfigElement.WSURL, mobilePushEmailUnsubscribeRequestData.Email);

        ICollection<KeyValuePair<string, string>> requestHeaders = new Collection<KeyValuePair<string, string>>();
        requestHeaders.Add(new KeyValuePair<string, string>("X-Subscription-Id", mobilePushEmailUnsubscribeRequestData.SubscriptionId.ToString()));

        IDictionary<string, string> responseHeaders;
        PostRequest(unsubscribeUrl, requestData.RequestTimeout, requestHeaders, out responseHeaders);

        string subscriptionErrorHeaderValue;
        if (!responseHeaders.TryGetValue("X-Subscription-Failure", out subscriptionErrorHeaderValue))
        {
          responseData = new MobilePushEmailUnsubscribeResponseData();
        }
        else
        {
          throw new Exception(string.Format("Unsubscribe failure: {0}", subscriptionErrorHeaderValue));
        }
      }
      catch (Exception ex)
      {
        responseData = new MobilePushEmailUnsubscribeResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
