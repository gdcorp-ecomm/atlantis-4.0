using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShortUrl.Interface;

namespace Atlantis.Framework.ShortUrl.Impl
{
  public class ShortUrlRequest : IRequest
  {
    private static string BuildUrl(ShortUrlRequestData requestData, ConfigElement config)
    {
      string apiKey = config.GetConfigValue("ApiKey");
      string requestUrl = ((WsConfigElement)config).WSURL;

      if(!requestUrl.EndsWith("/"))
      {
        requestUrl = requestUrl + "/";
      }

      StringBuilder urlBuilder = new StringBuilder();
      urlBuilder.AppendFormat("{0}text/{1}?url={2}", requestUrl,
                                                      apiKey,
                                                      HttpUtility.UrlEncode(requestData.Url));

      return urlBuilder.ToString();
    }

    private static HttpWebRequest CreateWebRequest(string url, string method, string userAgent, TimeSpan requestTimeout)
    {
      bool success = Uri.IsWellFormedUriString(url, UriKind.Absolute);

      if (!success)
      {
        throw new WebException("Invalid web service url: " + url);
      }

      Uri requestUri = new Uri(url);

      HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
      webRequest.Proxy = null;
      webRequest.Method = method;
      webRequest.KeepAlive = false;
      webRequest.ContentType = "text/plain";
      webRequest.AllowAutoRedirect = false;
      webRequest.UserAgent = userAgent;
      webRequest.Timeout = (int)requestTimeout.TotalMilliseconds;

      return webRequest;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      ShortUrlRequestData shortUrlRequestData = (ShortUrlRequestData)requestData;

      if (string.IsNullOrEmpty(shortUrlRequestData.Url))
      {
        throw new WebException("Url property of ShortUrlRequestData cannot be null or empty.");
      }
      
      string webServiceUrl = BuildUrl(shortUrlRequestData, config);

      HttpWebResponse webResponse = null;
      Stream webResponseDataStream = null;
      StreamReader webResponseStreamReader = null;

      try
      {
        HttpWebRequest webRequest = CreateWebRequest(webServiceUrl,
                                                     WebRequestMethods.Http.Get,
                                                     "Atlantis.Framework.ShortUrl.Request",
                                                     shortUrlRequestData.RequestTimeout);

        webResponse = (HttpWebResponse)webRequest.GetResponse();
        webResponseDataStream = webResponse.GetResponseStream();

        if (webResponseDataStream == null)
        {
          throw new WebException(string.Format("Request to {0} failed. Response stream is null.", webServiceUrl));
        }

        webResponseStreamReader = new StreamReader(webResponseDataStream);
        string responseFromServer = webResponseStreamReader.ReadToEnd();

        responseData = new ShortUrlResponseData(responseFromServer, shortUrlRequestData);
      }
      catch (Exception ex)
      {
        responseData = new ShortUrlResponseData(new Exception(string.Format("Web service call to {0} failed. {1}.", webServiceUrl, ex.Message)), shortUrlRequestData);
      }
      finally
      {
        if (webResponseStreamReader != null)
        {
          webResponseStreamReader.Close();
          webResponseStreamReader.Dispose();
        }

        if (webResponseDataStream != null)
        {
          webResponseDataStream.Close();
          webResponseDataStream.Dispose();
        }

        if (webResponse != null)
        {
          webResponse.Close();
        }
      }

      return responseData;
    }
  }
}
