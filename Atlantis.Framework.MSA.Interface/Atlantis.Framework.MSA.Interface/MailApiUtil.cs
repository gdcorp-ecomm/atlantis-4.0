using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Atlantis.Framework.MSA.Interface
{
  public class MailApiUtil
  {
    private const string COOKIE_STRING = "{{\"session\":\"{0}\",\"app_key\":\"{1}\"}}";
    private const string COOKIE_STRING_RESTRICTED = "{{\"session\":\"{0}\",\"app_key\":\"{1}\",\"key\":\"{2}\"}}";    

    public static bool IsInvalidSession(MailApiResponse response)
    {
      return IsJsoapFaultMsgPresent(response, "INVALID_SESSION");
    }

    public static bool IsInvalidFolder(MailApiResponse response)
    {
      return IsJsoapFaultMsgPresent(response, "INVALID_FOLDER");
    }

    private static bool IsJsoapFaultMsgPresent(MailApiResponse response, string msg)
    {
      if (response != null && response.isJsoapFault())
      {
        if (!String.IsNullOrEmpty(response.getJsoapFaultMessage()) &&
            response.getJsoapFaultMessage().ToUpperInvariant().Contains(msg))
          return true;
      }
      return false;
    }


    public static HttpWebResponse sendMailAPIRequest(string mailApiUrl, string messageBody, string mailHash,
                                                     string appKey, string restrictedKey)
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mailApiUrl);
      request.Method = WebRequestMethods.Http.Post;

      byte[] bodyBytes = Encoding.UTF8.GetBytes(messageBody);

      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = bodyBytes.Length;

      if (!string.IsNullOrEmpty(restrictedKey))
      {
        AddRestrictedStateCookieToRequest(mailApiUrl, mailHash, appKey, request, restrictedKey);        
      }
      else if( !string.IsNullOrEmpty(mailHash))
      {        
        AddStateCookieToRequest(mailApiUrl, mailHash, appKey, request);
      }

      Stream requestStream = request.GetRequestStream();
      requestStream.Write(bodyBytes, 0, bodyBytes.Length);
      requestStream.Close();

      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      return response;
    }

    public static HttpWebResponse sendMailAPIRequest(string mailApiUrl, string messageBody, string mailHash, string appKey)
    {
      return sendMailAPIRequest(mailApiUrl, messageBody, mailHash, appKey, null);
    }

    /**
     *  create session cookie for given URL, using provided mailHash, and add to request.  iOS/Android app key added
     */
    private static void AddStateCookieToRequest(string url, string mailHash, string appKey, HttpWebRequest request)
    {
        string encodedCookieValue = HttpUtility.UrlEncode(String.Format(COOKIE_STRING, mailHash, appKey));
        Cookie sessionCookie = new Cookie("state", encodedCookieValue);
        request.CookieContainer = new CookieContainer();
        request.CookieContainer.Add(new Uri(url), sessionCookie);
    }

    /**
    *  create session cookie for given URL, using provided mailHash, and add to request.  iOS/Android app key added.  Restricted access key added.
    */
    private static void AddRestrictedStateCookieToRequest(string url, string mailHash, string appKey, HttpWebRequest request, string restrictedKey)
    {
      string encodedCookieValue = HttpUtility.UrlEncode(String.Format(COOKIE_STRING_RESTRICTED, mailHash, appKey, restrictedKey));
      Cookie sessionCookie = new Cookie("state", encodedCookieValue);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(url), sessionCookie);
    }
  }
}
