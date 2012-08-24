using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobileAir2WebSms.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MobileAir2WebSms.Impl
{
  public class MobileAir2WebSmsRequest : IRequest
  {
    #region Helper Methods

    private static bool GetConnectionCredentials(string nimitzAuthXml, out string userName, out string password)
    {
      bool success = false;
      userName = string.Empty;
      password = string.Empty;

      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(nimitzAuthXml);

      XmlNode userNameNode = xdoc.SelectSingleNode("Connect/UserID");
      XmlNode passwordNode = xdoc.SelectSingleNode("Connect/Password");

      if (userNameNode != null &&
          passwordNode != null)
      {
        userName = userNameNode.FirstChild.Value;
        password = passwordNode.FirstChild.Value;

        success = true;
      }

      return success;
    }

    private static string BuildUrl(MobileAir2WebSmsRequestData requestData, ConfigElement config)
    {
      string replyTo = config.GetConfigValue("ReplyTo");
      string requestUrl = ((WsConfigElement)config).WSURL;

      if(!requestUrl.ToLower().StartsWith("https:"))
      {
        throw new Exception("Request must be made over \"https\".");
      }

      StringBuilder urlBuilder = new StringBuilder(requestUrl);
      urlBuilder.AppendFormat("?{0}={1}&{2}={3}&{4}={5}", "reply_to",
                                                          HttpUtility.UrlEncode(replyTo),
                                                          "recipient",
                                                          HttpUtility.UrlEncode(requestData.Recipient),
                                                          "body",
                                                          HttpUtility.UrlEncode(requestData.Body));

      if (!string.IsNullOrEmpty(requestData.ReportingKey1))
      {
        urlBuilder.AppendFormat("&{0}={1}", "reporting_key1", HttpUtility.UrlEncode(requestData.ReportingKey1));
      }

      if (!string.IsNullOrEmpty(requestData.ReportingKey2))
      {
        urlBuilder.AppendFormat("&{0}={1}", "reporting_key2", HttpUtility.UrlEncode(requestData.ReportingKey2));
      }

      return urlBuilder.ToString();
    }

    private static HttpWebRequest CreateWebRequest(string url, string method, string userAgent, NetworkCredential credentials, TimeSpan requestTimeout)
    {
      bool success = Uri.IsWellFormedUriString(url, UriKind.Absolute);

      if (!success)
      {
        throw new WebException("Invalid sms url: " + url);
      }
      
      Uri requestUri = new Uri(url);

      HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
      webRequest.Proxy = null;
      webRequest.Method = method;
      webRequest.KeepAlive = false;
      webRequest.ContentType = "text/html";
      webRequest.AllowAutoRedirect = false;
      webRequest.UserAgent = userAgent;
      webRequest.Credentials = credentials;
      webRequest.Timeout = (int)requestTimeout.TotalMilliseconds;

      return webRequest;
    }

    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      MobileAir2WebSmsRequestData mobileAir2WebSmsRequestData = (MobileAir2WebSmsRequestData) requestData;

      string userName;
      string password;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);
      if(!GetConnectionCredentials(nimitzAuthXml, out userName, out password))
      {
        responseData = new MobileAir2WebSmsResponseData(new Exception("Unable to retreive nimitz credentials."), requestData);
      }
      else
      {
        string postUrl = BuildUrl(mobileAir2WebSmsRequestData, config);

        HttpWebResponse webResponse = null;
        Stream webResponseDataStream = null;
        StreamReader webResponseStreamReader = null;

        try
        {
          HttpWebRequest webRequest = CreateWebRequest(postUrl,
                                                       WebRequestMethods.Http.Post,
                                                       "MobileAir2WebSmsRequest",
                                                       new NetworkCredential(userName, password),
                                                       mobileAir2WebSmsRequestData.RequestTimeout);

          webResponse = (HttpWebResponse)webRequest.GetResponse();
          webResponseDataStream = webResponse.GetResponseStream();

          if (webResponseDataStream == null)
          {
            throw new WebException(string.Format("Post to {0} failed. Response stream is null.", ((WsConfigElement)config).WSURL));
          }

          webResponseStreamReader = new StreamReader(webResponseDataStream);
          string responseFromServer = webResponseStreamReader.ReadToEnd();

          responseData = new MobileAir2WebSmsResponseData(responseFromServer, requestData);
        }
        catch (Exception ex)
        {
          responseData = new MobileAir2WebSmsResponseData(new Exception(string.Format("SMS call to {0} failed. {1}.", ((WsConfigElement)config).WSURL, ex.Message)), requestData);
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
      }

      return responseData;
    }
  }
}
