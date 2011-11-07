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

    private static string PostNotification(string url, TimeSpan requestTimeout, string authToken, byte[] postData, out HttpStatusCode httpStatusCode)
    {
      string response = string.Empty;
      httpStatusCode = HttpStatusCode.BadRequest;

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
          httpStatusCode = httpResponse.StatusCode;

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

      MobileAndroidPushRequestData mobileAndroidPushRequestData = (MobileAndroidPushRequestData) requestData;
      WsConfigElement wsConfig = (WsConfigElement)config;

      try
      {
        HttpStatusCode statusCode;
        string response = PostNotification(wsConfig.WSURL, 
                                           mobileAndroidPushRequestData.RequestTimeout, 
                                           mobileAndroidPushRequestData.AuthToken, 
                                           mobileAndroidPushRequestData.Notification.GetPostData(),
                                           out statusCode);


        responseData = new MobileAndroidPushResponseData(response, false, false);
      }
      catch(WebException webException)
      {
        bool authError = false;
        bool serviceUnavailable = false;

        HttpWebResponse response = webException.Response as HttpWebResponse;
        if(response == null)
        {
          throw;
        }
        
        switch (response.StatusCode)
        {
          case HttpStatusCode.Unauthorized:
            authError = true;
            break;
          case HttpStatusCode.ServiceUnavailable:
            serviceUnavailable = true;
            break;
          default:
            throw;
        }

        responseData = new MobileAndroidPushResponseData(string.Empty, authError, serviceUnavailable);
      }
      catch (Exception ex)
      {
        responseData = new MobileAndroidPushResponseData(mobileAndroidPushRequestData, ex);
      }

      return responseData;
    }
  }
}
