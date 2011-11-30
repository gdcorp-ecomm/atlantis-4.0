using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CDS.Impl
{
  public class CDSRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CDSResponseData result = null;
      var cdsRequestData = requestData as CDSRequestData;
      string responseText = string.Empty;

      var wsConfig = ((WsConfigElement)config);
      var webRequest = WebRequest.Create(wsConfig.WSURL + cdsRequestData.Query) as HttpWebRequest;
      HttpWebResponse webResponse = null;
      try
      {
        if (webRequest != null)
        {
          webRequest.Method = "GET";
          webRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
          webResponse = webRequest.GetResponse() as HttpWebResponse;

          if (webResponse != null)
          {
            using (Stream webResponseData = webResponse.GetResponseStream())
            {
              if (webResponseData != null)
              {
                using (StreamReader responseReader = new StreamReader(webResponseData))
                {
                  responseText = responseReader.ReadToEnd();
                  responseReader.Close();
                }
              }
            }
          }
          result = new CDSResponseData(responseText, webResponse.StatusCode); 
        }
      }
      catch (WebException ex)
      {
        result = new CDSResponseData(ex.Message, ((HttpWebResponse)ex.Response).StatusCode);
        throw;
      }
      catch (Exception ex)
      {
        result = new CDSResponseData(requestData, webResponse.StatusCode, ex);
        throw;
      }
      return result;
    }
  }
}
