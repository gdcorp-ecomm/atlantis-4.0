using System.IO;
using System.Net;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DotTypeForms.Interface;
using System;

namespace Atlantis.Framework.DotTypeForms.Impl
{
  public class DotTypeFormsXmlRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeFormsXmlResponseData result;
      Stream post = null;
      string response = string.Empty;

      try
      {
        var requestBody = new StringBuilder();

        var searchData = ((DotTypeFormsXmlRequestData)requestData).ToJson();

        requestBody.Append(searchData);

        var buffer = Encoding.UTF8.GetBytes(requestBody.ToString());

        var wsConfigElement = ((WsConfigElement) config);
        var wsUrl = wsConfigElement != null ? wsConfigElement.WSURL : string.Empty;
        if (!string.IsNullOrEmpty(wsUrl))
        {
          wsUrl = wsUrl.TrimEnd('/');
          wsUrl = string.Format("{0}{1}", wsUrl, "/api/schema/FetchXml2");
        }

        var webRequest = WebRequest.Create(wsUrl) as HttpWebRequest;

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
                response = responseReader.ReadToEnd().Trim();
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

        result = new DotTypeFormsXmlResponseData(response);
      }
      catch (WebException ex)
      {
        if (ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
        {
          //no error logging is required
          result = new DotTypeFormsXmlResponseData(string.Empty);
        }
        else
        {
          var exception = new AtlantisException(requestData, "DotTypeFormsXmlRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
          result = new DotTypeFormsXmlResponseData(response, exception);
        }
      }
      catch (Exception ex)
      {
        result = new DotTypeFormsXmlResponseData(response, requestData, ex);
      }

      return result;
    }
  }
}
