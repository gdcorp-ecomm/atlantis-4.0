using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Atlantis.Framework.MailApi.Impl
{
  class Utility
  {

    public static string LoginRequest(string url, string messageBody)
    {
      var request = (HttpWebRequest)WebRequest.Create(url);
      request.Method = WebRequestMethods.Http.Post;
      return PostRequest(request, url, messageBody);
    }

    public static string GetMessageListRequest(string url, string messageBody, string appKey, string mailHash)
    {
      var request = (HttpWebRequest)WebRequest.Create(url);
      request.Method = WebRequestMethods.Http.Post;

      // Add cookies required for messageList

      return PostRequest(request, url, messageBody);
    }

    public static string PostRequest(HttpWebRequest request, string url, string messageBody)
    {
      string jsonResponse = null;

      byte[] bodyBytes = Encoding.UTF8.GetBytes(messageBody);

      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = bodyBytes.Length;

      using (var requestStream = request.GetRequestStream())
      {
        requestStream.Write(bodyBytes, 0, bodyBytes.Length);
        requestStream.Close();
      }

      var loginResponse = (HttpWebResponse)request.GetResponse();

      using (var responseStream = loginResponse.GetResponseStream())
      {
        if (responseStream != null)
        {
          using (var responseReader = new StreamReader(responseStream, Encoding.UTF8))
          {
            jsonResponse = responseReader.ReadToEnd();
            responseReader.Close();
          }
          responseStream.Close();
        }
      }

      return jsonResponse;
    }

  }
}
