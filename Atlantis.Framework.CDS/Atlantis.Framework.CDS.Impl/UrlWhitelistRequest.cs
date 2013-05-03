using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Atlantis.Framework.CDS.Impl
{
  public class UrlWhitelistRequest : IRequest
  {
    const string ApplicationNameKey = "ApplicationName";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      UrlWhitelistResponseData result = null;
      CDSRequestData cdsRequestData = requestData as CDSRequestData;
      WsConfigElement wsConfig = (WsConfigElement)config;
      cdsRequestData.AppName = wsConfig.GetConfigValue(ApplicationNameKey); //used to identify the App in the errorlog entry

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);
      try
      {
        string responseText= service.GetWebResponse();
        if (!string.IsNullOrEmpty(responseText))
        {
          result = new UrlWhitelistResponseData(responseText);
        }
        else
        {
          result = new UrlWhitelistResponseData(cdsRequestData, new Exception("Empty response from the CDS service."));
        }
      }
      catch (Exception ex)
      {
        result = new UrlWhitelistResponseData(cdsRequestData, ex);
        throw;
      }
      return result;
    }
  }
}
