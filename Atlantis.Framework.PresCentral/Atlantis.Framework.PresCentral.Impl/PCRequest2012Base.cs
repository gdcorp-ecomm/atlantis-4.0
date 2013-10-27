using System;
using System.Globalization;
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
      IResponseData result;

      var pcRequestData = (T)requestData;
      var serviceConfig = (WsConfigElement)config;
      var request = GetWebRequest(serviceConfig, pcRequestData);

      string output;
      using (var response = request.GetResponse())
      {
        using (var receiveStream = response.GetResponseStream())
        {
          using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
          {
            output = readStream.ReadToEnd();
          }
        }
      }

      var responseData = new PCResponse(output);

      if (responseData.ResultCode == 0)
      {
        result = pcRequestData.CreateResponse(responseData);
      }
      else
      {
        // Error conditions
        var mb = new StringBuilder(500);
        foreach (var error in responseData.Errors)
        {
          mb.Append(error.ErrorNumber.ToString(CultureInfo.InvariantCulture));
          mb.Append('=');
          mb.Append(error.Message);
          mb.Append(':');
        }

        throw new AtlantisException(requestData, "PCRequest2012.RequestHandler", mb.ToString(), request.RequestUri.ToString());
      }

      return result;
    }

    private static WebRequest GetWebRequest(WsConfigElement config, T requestData)
    {
      var urlBuilder = new UriBuilder(config.WSURL);
      var query = requestData.GetQuery();

      if (!string.IsNullOrEmpty(query))
      {
        urlBuilder.Query = query;
      }

      var result = WebRequest.Create(urlBuilder.Uri);
      result.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      result.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
      return result;
    }
  }
}
