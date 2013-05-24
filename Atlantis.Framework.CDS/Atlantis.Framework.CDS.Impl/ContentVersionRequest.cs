using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.CDS.Impl
{
  public class ContentVersionRequest : IRequest
  {
    const string ApplicationNameKey = "ApplicationName";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ContentVersionResponseData result = null;
      CDSRequestData cdsRequestData = requestData as CDSRequestData;
      WsConfigElement wsConfig = (WsConfigElement)config;
      cdsRequestData.AppName = wsConfig.GetConfigValue(ApplicationNameKey); //used to identify the App in the errorlog entry

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);
      try
      {
        string responseText = service.GetWebResponse();
        if (!string.IsNullOrEmpty(responseText))
        {
          result = new ContentVersionResponseData(responseText);
        }
        else
        {
          result = new ContentVersionResponseData(cdsRequestData, new Exception("Empty response from the CDS service."));
        }
      }
      catch (Exception ex)
      {
        result = new ContentVersionResponseData(cdsRequestData, ex);
      }

      return result;
    }
  }
}
