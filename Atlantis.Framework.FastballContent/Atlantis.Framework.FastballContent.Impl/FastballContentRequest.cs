using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Atlantis.Framework.FastballContent.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballContent.Impl
{
  public class FastballContentRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      FastballContentResponseData result = null;
      var fbRequestData = requestData as FastballContentRequestData;

      try
      {
        if (fbRequestData == null)
        {
          throw new Exception("FastballContentRequestData requestData is null");
        }

        string requestUrl = fbRequestData.RequestUrl;
        CookieContainer cookieJar = new CookieContainer();
        Uri uri = new Uri(requestUrl);

        foreach (Cookie cookie in fbRequestData.GetRequestCookies())
        {
          cookieJar.Add(uri, cookie);
        }

        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
        webRequest.Timeout = (int)Math.Truncate(requestData.RequestTimeout.TotalMilliseconds);
        webRequest.CookieContainer = cookieJar;

        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

        string responseText;
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
          responseText = reader.ReadToEnd();
        }

        result = new FastballContentResponseData(responseText); 

        using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseText)))
        {
          DataContractJsonSerializer jsonDeserializer = new DataContractJsonSerializer(typeof(List<OfferResponse>));
          List<OfferResponse> data = (List<OfferResponse>)jsonDeserializer.ReadObject(stream);
          if (data != null)
          {
            ((FastballContentResponseData)result).OfferResponses.AddRange(data);
          }
        }
       
      }
      catch (Exception ex)
      {
        result = new FastballContentResponseData(requestData, ex);
      }

      return result;
    }
  }
}
