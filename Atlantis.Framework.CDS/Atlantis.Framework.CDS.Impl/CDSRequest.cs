using System;
using System.IO;
using System.Net;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CDS.Impl
{
  public class CDSRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CDSResponseData result = null;
      HttpWebResponse response = null;
      var cdsRequestData = requestData as CDSRequestData;
      string responseText;

      var wsConfig = ((WsConfigElement)config);
      var webRequest = WebRequest.Create(wsConfig.WSURL + cdsRequestData.Query) as HttpWebRequest;

      try
      {
        if (webRequest != null)
        {
          webRequest.Method = "GET";
          response = (HttpWebResponse)webRequest.GetResponse();

          
          using (StreamReader reader = new StreamReader(response.GetResponseStream()))
          {
            responseText = reader.ReadToEnd();
          }
          result = new CDSResponseData(responseText, response.StatusCode); 
        }
      }
      catch (WebException ex)
      {
        result = new CDSResponseData(ex.Message, ((HttpWebResponse)ex.Response).StatusCode);
      }
      catch (Exception ex)
      {
        result = new CDSResponseData(requestData, response.StatusCode, ex);
      }
      return result;
    }
  }
}
