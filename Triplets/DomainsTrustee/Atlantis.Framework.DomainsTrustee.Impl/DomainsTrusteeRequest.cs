using System;
using System.IO;
using System.Net;
using System.Text;
using Atlantis.Framework.DomainsTrustee.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsTrustee.Impl
{
  public class DomainsTrusteeRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result;
      Stream post = null;

      try
      {
        var requestBody = new StringBuilder();
        string response = null;

        var searchData = ((DomainsTrusteeRequestData)requestData).ToJson();

        requestBody.Append(searchData);

        var buffer = Encoding.UTF8.GetBytes(requestBody.ToString());
        var webRequest = WebRequest.Create(((WsConfigElement)config).WSURL) as HttpWebRequest;

        if (webRequest != null)
        {
          webRequest.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          webRequest.Method = "POST";
          webRequest.ContentType = "application/json";
          webRequest.ContentLength = buffer.Length;
          webRequest.KeepAlive = false;
          post = webRequest.GetRequestStream();
        }

        if (post != null)
        {
          post.Write(buffer, 0, buffer.Length);
          post.Close();
        }

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

        result = DomainsTrusteeResponseData.ParseRawResponse(response);
      }
      catch (Exception ex)
      {
        var message = ex.Message + " - " + ex.StackTrace;
        var aex = new AtlantisException(requestData, "DomainsTrusteeRequest.RequestHandler", message, requestData.ToXML());

        result = DomainsTrusteeResponseData.FromAtlantisException(aex);
      }
      finally
      {
        if (post != null)
        {
          post.Dispose();
        }
      }

      return result;
    }
  }
}
