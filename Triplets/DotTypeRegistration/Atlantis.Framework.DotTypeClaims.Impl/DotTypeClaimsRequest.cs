using System;
using System.IO;
using System.Net;
using System.Text;
using Atlantis.Framework.DotTypeClaims.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeClaims.Impl
{
  public class DotTypeClaimsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeClaimsResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
        var dotTypeClaimsRequestData = (DotTypeClaimsRequestData)requestData;
        var url = ((WsConfigElement)config).WSURL;

        var fullUrl = url + "/getclaimdata?d=" + string.Join(",", dotTypeClaimsRequestData.Domains);

        var webRequest = (HttpWebRequest) WebRequest.Create(fullUrl);
        webRequest.ContentType = "application/x-www-form-urlencoded";
        webRequest.Method = "GET";
        webRequest.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
        webRequest.KeepAlive = false;

        var webResponse = webRequest.GetResponse();

        var dataStream = webResponse.GetResponseStream();
        if (dataStream != null)
        {
          var streamReader = new StreamReader(dataStream);
          responseXml = streamReader.ReadToEnd().Trim();
          streamReader.Close();
        }
        webResponse.Close();

        responseData = DotTypeClaimsResponseData.FromResponseXml(responseXml);
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException(requestData, "DotTypeClaimsRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        responseData = DotTypeClaimsResponseData.FromException(exception);
      }

      return responseData;
    }
  }
}
