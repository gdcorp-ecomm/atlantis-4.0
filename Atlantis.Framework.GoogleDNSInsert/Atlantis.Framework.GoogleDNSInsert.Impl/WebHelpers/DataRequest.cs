using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Collections.Specialized;

namespace Atlantis.Framework.Google
{
  internal static class DataRequest
  {

    public static string PostRequest(string requestUrl, string postBody, string ContentType, TimeSpan requestTimeout, RequestCacheLevel cacheLevel)
    {
      bool isSuccess;
      return PostRequest(requestUrl, postBody, ContentType, null, requestTimeout, cacheLevel, out isSuccess);
    }
    
    public static string PostRequest(string requestUrl, string postBody, string ContentType, NameValueCollection header, TimeSpan requestTimeout, RequestCacheLevel cacheLevel, out bool isSuccess)
    {
      var response = string.Empty;
      var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
      isSuccess = false;
      if (webRequest != null)
      {
        webRequest.Timeout = (int)requestTimeout.TotalMilliseconds;
        webRequest.Method = "POST";
        webRequest.ContentType = ContentType;
        webRequest.CachePolicy = new RequestCachePolicy(cacheLevel);
        
        if (header !=null)
        webRequest.Headers.Add(header);

        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] data = encoding.GetBytes(postBody);

        Stream newStream;
        try
        {
          newStream = webRequest.GetRequestStream();
          newStream.Write(data, 0, data.Length);
          newStream.Close();


          var webResponse = webRequest.GetResponse() as HttpWebResponse;
          if (webResponse != null)
          {
            using (Stream webResponseData = webResponse.GetResponseStream())
            {
              if (webResponseData != null)
              {
                using (StreamReader responseReader = new StreamReader(webResponseData))
                {
                  response = responseReader.ReadToEnd();
                  responseReader.Close();
                  isSuccess = true;
                }
              }
            }
          }
        }
        catch (System.Net.WebException we)
        {

          StringBuilder s = new StringBuilder();
          int value = we.Response.GetResponseStream().ReadByte();
          while (value != -1 && value < 1024) //read first 1k bytes
          {
            s.Append((char)value);
            value = we.Response.GetResponseStream().ReadByte();

          }
          response = s.ToString();
          isSuccess = false;
        }
      }

      return response;
    }
  }
}