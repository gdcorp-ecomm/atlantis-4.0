using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Atlantis.Framework.GoogleClientAuth.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.GoogleClientAuth.Impl
{
  public class GoogleClientAuthRequest : IRequest
  {
    private const string POST_DATA_FIRST_ITEM_FORMAT = "{0}={1}";
    private const string POST_DATA_ITEM_FORMAT = "&{0}={1}";

    private const string ACCOUNT_TYPE_KEY = "accountType";
    private const string EMAIL_KEY = "Email";
    private const string PASSWORD_KEY = "Passwd";
    private const string SERVICE_TYPE_KEY = "service";
    private const string SOURCE_APPLICATION_KEY = "source";

    private static bool GetGoogleCredentials(string authXml, out string userName, out string password)
    {
      bool success = false;
      userName = string.Empty;
      password = string.Empty;

      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(authXml);

      XmlNode userIdNode = xdoc.SelectSingleNode("Connect/UserID");
      XmlNode passwordNode = xdoc.SelectSingleNode("Connect/Password");

      if (userIdNode != null &&
          passwordNode != null)
      {
        userName = userIdNode.FirstChild.Value;
        password = passwordNode.FirstChild.Value;
        success = true;
      }

      return success;
    }

    private static string PostCredentials(string authXml, string url, string serviceType, string accountType, string sourceApplication, TimeSpan requestTimeout)
    {
      string response = string.Empty;

      if (!url.StartsWith("https://"))
      {
        throw new Exception("Client Auth Url must make requests over https.");
      }

      string userName;
      string password;
      if(!GetGoogleCredentials(authXml, out userName, out password))
      {
        throw new Exception("Unable to look up google credentials.");
      }

      StringBuilder postDataBuilder = new StringBuilder();
      postDataBuilder.AppendFormat(POST_DATA_FIRST_ITEM_FORMAT, SERVICE_TYPE_KEY, serviceType);
      postDataBuilder.AppendFormat(POST_DATA_ITEM_FORMAT, ACCOUNT_TYPE_KEY, accountType);
      postDataBuilder.AppendFormat(POST_DATA_ITEM_FORMAT, EMAIL_KEY, userName);
      postDataBuilder.AppendFormat(POST_DATA_ITEM_FORMAT, PASSWORD_KEY, password);
      postDataBuilder.AppendFormat(POST_DATA_ITEM_FORMAT, SOURCE_APPLICATION_KEY, sourceApplication);

      byte[] postData = Encoding.ASCII.GetBytes(postDataBuilder.ToString());

      HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
      httpRequest.Method = "POST";
      httpRequest.ContentType = "application/x-www-form-urlencoded";
      httpRequest.ContentLength = postData.Length;

      httpRequest.Timeout = (int)requestTimeout.TotalMilliseconds;

      using (Stream requestStream = httpRequest.GetRequestStream())
      {
        requestStream.Write(postData, 0, postData.Length);

        using (HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse())
        {
          using (Stream responseStream = httpResponse.GetResponseStream())
          {
            if (responseStream != null)
            {
              using (StreamReader responseStreamReader = new StreamReader(responseStream))
              {
                response = responseStreamReader.ReadToEnd();
              }
            }
          }
        }
      }

      return response;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      GoogleClientAuthRequestData mobileAndroidPushRequestData = (GoogleClientAuthRequestData) requestData;
      WsConfigElement wsConfig = (WsConfigElement)config;

      try
      {
        string authXml = NetConnect.LookupConnectInfo(wsConfig, ConnectLookupType.Xml);
        string response = PostCredentials(authXml, 
                                          wsConfig.WSURL,
                                          mobileAndroidPushRequestData.ServiceType,
                                          mobileAndroidPushRequestData.AccountType,
                                          mobileAndroidPushRequestData.SourceApplication,
                                          mobileAndroidPushRequestData.RequestTimeout);


        responseData = new GoogleClientAuthResponseData(response, false);
      }
      catch(WebException webException)
      {
        bool serviceUnavailable = false;

        HttpWebResponse response = webException.Response as HttpWebResponse;
        if(response == null)
        {
          throw;
        }
        
        switch (response.StatusCode)
        {
          case HttpStatusCode.ServiceUnavailable:
            serviceUnavailable = true;
            break;
          default:
            throw;
        }

        responseData = new GoogleClientAuthResponseData(string.Empty, serviceUnavailable);
      }
      catch (Exception ex)
      {
        responseData = new GoogleClientAuthResponseData(mobileAndroidPushRequestData, ex);
      }

      return responseData;
    }
  }
}
