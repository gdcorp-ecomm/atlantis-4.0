using System;
using System.IO;
using System.Net;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobileAndroidPush.Interface;

namespace Atlantis.Framework.MobileAndroidPush.Impl
{
  public class MobileAndroidPushRequest : IRequest
  {
    public const string CLIENT_AUTH_HEADER = "Authorization"; 
    public const string CLIENT_AUTH_HEADER_VALUE_FORMAT = "GoogleLogin auth={0}";

    private GoogleClientAuthCache _googleClientAuthCache = new GoogleClientAuthCache();

    private static string PostNotification(string url, string authToken, TimeSpan requestTimeout, byte[] postData)
    {
      string response = string.Empty;

      ServicePointManager.ServerCertificateValidationCallback += GoogleCertificatePolicy.ValidateRemoteCertificate;

      HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
      httpRequest.Method = "POST";
      httpRequest.ContentType = "application/x-www-form-urlencoded";
      httpRequest.ContentLength = postData.Length;
      

      httpRequest.Timeout = (int)requestTimeout.TotalMilliseconds;

      httpRequest.Headers.Add(CLIENT_AUTH_HEADER, string.Format(CLIENT_AUTH_HEADER_VALUE_FORMAT, authToken));

      using(Stream requestStream = httpRequest.GetRequestStream())
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

    private IResponseData SendNotification(MobileAndroidPushRequestData mobileAndroidPushRequestData, WsConfigElement wsConfig)
    {
      return SendNotification(mobileAndroidPushRequestData, wsConfig, false);
    }

    private IResponseData SendNotification(MobileAndroidPushRequestData mobileAndroidPushRequestData, WsConfigElement wsConfig, bool clearClientAuthCache)
    {
      IResponseData responseData;

      try
      {
        _googleClientAuthCache.ClearCache = clearClientAuthCache;
        string authToken = _googleClientAuthCache.GetClientAuthToken(mobileAndroidPushRequestData);
        
        string response = PostNotification(wsConfig.WSURL,
                                           authToken,
                                           mobileAndroidPushRequestData.RequestTimeout,
                                           mobileAndroidPushRequestData.Notification.GetPostData());


        responseData = new MobileAndroidPushResponseData(response);
      }
      catch (WebException webException)
      {
        HttpWebResponse response = webException.Response as HttpWebResponse;
        if (response == null)
        {
          throw;
        }

        switch (response.StatusCode)
        {
          case HttpStatusCode.Unauthorized:
            if (!clearClientAuthCache)
            {
              responseData = SendNotification(mobileAndroidPushRequestData, wsConfig, true);
            }
            else
            {
              throw new Exception("Google Client Authentication failed.", webException);
            }
            break;
          default:
            throw;
        }
      }
      catch (Exception ex)
      {
        responseData = new MobileAndroidPushResponseData(mobileAndroidPushRequestData, ex);
      }

      return responseData;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MobileAndroidPushRequestData mobileAndroidPushRequestData = (MobileAndroidPushRequestData) requestData;
      WsConfigElement wsConfig = (WsConfigElement)config;

      return SendNotification(mobileAndroidPushRequestData, wsConfig);
    }
  }
}
