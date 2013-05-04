using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CDS.Impl
{
  public class CDSRequest : IRequest
  {
    const string ApplicationNameKey = "ApplicationName";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CDSResponseData result = null;
      CDSRequestData cdsRequestData = requestData as CDSRequestData;
      WsConfigElement wsConfig = (WsConfigElement)config;
      cdsRequestData.AppName = wsConfig.GetConfigValue(ApplicationNameKey); //used to identify the App in the errorlog entry

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);
      try
      {
        string responseText = service.GetWebResponse();
        result = new CDSResponseData(responseText);
      }
      catch (WebException ex)
      {
        if (ex.Response != null && ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
        {
          result = new CDSResponseData(ex.Message, false);
        }
        else
        {
          result = new CDSResponseData(cdsRequestData, ex);
          throw;
        }        
      }
      catch (Exception ex)
      {
        result = new CDSResponseData(cdsRequestData, ex);
        throw;
      }
      return result;
    }
  }
}
