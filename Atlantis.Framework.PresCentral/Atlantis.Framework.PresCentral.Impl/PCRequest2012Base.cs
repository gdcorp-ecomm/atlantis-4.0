using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PresCentral.Interface;

namespace Atlantis.Framework.PresCentral.Impl
{
  public abstract class PCRequest2012Base<T> : IRequest 
    where T: PCRequestDataBase 
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      T pcRequestData = (T)requestData;
      WsConfigElement serviceConfig = (WsConfigElement)config;
      WebRequest request = GetWebRequest(serviceConfig, pcRequestData);

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

        PCResponse responseData = new PCResponse(output);

        if ((responseData.ResultCode == 0) || (pcRequestData.AllowErrorResponses))
        {
          result = pcRequestData.CreateResponse(responseData);
        }
        else
        {
          // Error conditions
          StringBuilder mb = new StringBuilder(500);
          foreach (PCResponseError error in responseData.Errors)
          {
            mb.Append(error.ErrorNumber.ToString());
            mb.Append('=');
            mb.Append(error.Message);
            mb.Append(':');
          }

          string message = mb.ToString();
          string data = request.RequestUri.ToString();
          AtlantisException aex = new AtlantisException(requestData, "PCRequest2012.RequestHandler", message, data);
          result = pcRequestData.CreateResponse(aex);
        }

      }
      catch (Exception ex)
      {
        string message = ex.Message + ex.StackTrace;
        string data = request.RequestUri.ToString();
        AtlantisException aex = new AtlantisException(requestData, "PCRequest2012.RequestHandler", message, data);
        result = pcRequestData.CreateResponse(aex);
      }

      return result;
    }

    private WebRequest GetWebRequest(WsConfigElement config, T requestData)
    {
      WebRequest result;
      UriBuilder urlBuilder = new UriBuilder(config.WSURL);

      string query = requestData.GetQuery();
      if (!string.IsNullOrEmpty(query))
      {
        urlBuilder.Query = query;
      }

      Uri uri = urlBuilder.Uri;
      result = HttpWebRequest.Create(uri);
      result.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      result.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
      return result;
    }
  }
}
