using Atlantis.Framework.Interface;
using Atlantis.Framework.Purchase.Interface;
using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace Atlantis.Framework.Purchase.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.Purchase.PurchaseError" assembly="Atlantis.Framework.Purchase.dll" request_type="###" />

  public class PurchaseErrorRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result;
      PurchaseErrorRequestData purchaseErrorRequest = (PurchaseErrorRequestData)requestData;
      try
      {
        WsConfigElement currentConfig = (WsConfigElement)config;
        string url = CreateRequestURL(purchaseErrorRequest, currentConfig);
        string xml = GetServiceDataXml(url, requestData.RequestTimeout);
        result = PurchaseErrorResponseData.FromXML(xml, purchaseErrorRequest);
      }
      catch (Exception ex)
      {
        result = PurchaseErrorResponseData.FromException(new AtlantisException(requestData, "PurchaseErrors Retrieval", string.Empty, string.Empty, ex), purchaseErrorRequest);
      }
      return result;
    }

    private string CreateRequestURL(PurchaseErrorRequestData currentRequest, WsConfigElement currentConfig)
    {
      string restRequest = string.Empty;
      string urlDelim = "/";
      System.Text.StringBuilder requestURL = new StringBuilder(100);
      requestURL.Append(currentConfig.WSURL);
      requestURL.Append(currentRequest.ResponseCode);
      requestURL.Append(urlDelim);
      requestURL.Append(currentRequest.ReasonCode);
      requestURL.Append(urlDelim);
      if (!string.IsNullOrEmpty(currentRequest.LanguageCode) && !string.IsNullOrEmpty(currentRequest.RegionCode))
      {
        requestURL.Append(currentRequest.LanguageCode);
        requestURL.Append("_");
        requestURL.Append(currentRequest.RegionCode);
      }      
      return requestURL.ToString();
    }

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
