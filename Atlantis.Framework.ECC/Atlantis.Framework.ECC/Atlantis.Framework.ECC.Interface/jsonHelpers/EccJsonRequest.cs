using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Atlantis.Framework.Ecc.Interface.jsonHelpers
{
  public static class EccJsonRequestHandler
  {
    public static string PostRequest(string requestParams, string requestUrl, string requestMethod, string requestKey)
    {
      TimeSpan defaultTimeout = TimeSpan.FromSeconds(20);
      return PostRequest(requestParams, requestUrl, requestMethod, requestKey, defaultTimeout);
    }

    public static string PostRequest(string requestParams, string requestUrl, string requestMethod, string requestKey, TimeSpan requestTimeout)
    {
      string requestBody = string.Empty;
      const string method = "method={0}";
      const string parameters = "params={0}";
      const string key = "key={0}";

      string response = string.Empty;
      Stream post = null;

      if (string.IsNullOrEmpty(requestMethod))
      {
        throw new ArgumentNullException("requestMethod", "The parameter 'requestMethod' is required and cannot be null");
      }
      requestBody += string.Format(method, requestMethod);


      if (!string.IsNullOrEmpty(requestKey))
      {
        requestBody += "&" + string.Format(key, requestKey);
      }

      if (!string.IsNullOrEmpty(requestParams))
      {
        requestBody += "&" + string.Format(parameters, requestParams);
      }


      byte[] buffer = Encoding.ASCII.GetBytes(requestBody);
      var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
      
      webRequest.Timeout = (int)requestTimeout.TotalMilliseconds;
      try
      {
        if (webRequest != null)
        {
          webRequest.Method = "POST";
          webRequest.ContentType = "application/x-www-form-urlencoded";
          webRequest.ContentLength = buffer.Length;
          post = webRequest.GetRequestStream();
        }

        if (post != null)
        {
          post.Write(buffer, 0, buffer.Length);
          post.Close();
        }
      }
      finally
      {
        if (post != null) post.Dispose();
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
              if (responseReader != null) responseReader.Dispose();
            }

          }
        }
      }

      return response;
    }
  }

  public class EccJsonRequest<T>
  {
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Token { get; set; }

    [DataMember]
    public EccJsonPaging Return { get; set; }

    [DataMember(Name = "Parameters")]
    public T Parameters { get; set; }

    public string ToJson()
    {
      var ser = new DataContractJsonSerializer(typeof(EccJsonRequest<T>));
      var ms = new MemoryStream();
      ser.WriteObject(ms, this);

      string json = Encoding.Default.GetString(ms.ToArray());
      ms.Close();

      return "[" + json + "]";
    }

  }

  [DataContract(Name = "Return")]
  public class EccJsonPaging
  {
    public EccJsonPaging(int pageNumber, int resultsPerPage, string orderBy, string sortOrder)
    {
      PageNumber = pageNumber;
      ResultsPerPage = resultsPerPage;
      OrderBy = orderBy;
      SortOrder = sortOrder;
    }

    [DataMember]
    public int PageNumber { get; set; }

    [DataMember]
    public int ResultsPerPage { get; set; }

    [DataMember]
    public string OrderBy { get; set; }

    [DataMember]
    public string SortOrder { get; set; }
  }

}
