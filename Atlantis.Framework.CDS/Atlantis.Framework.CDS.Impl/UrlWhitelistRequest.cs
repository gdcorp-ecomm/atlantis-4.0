using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.CDS.Impl
{
  public class UrlWhitelistRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      UrlWhitelistResponseData result;

      CDSRequestData cdsRequestData = (CDSRequestData)requestData;

      WsConfigElement wsConfig = (WsConfigElement)config;

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);

      try
      {
        string responseText= service.GetWebResponse();

        result = !string.IsNullOrEmpty(responseText) ? new UrlWhitelistResponseData(responseText) : new UrlWhitelistResponseData(cdsRequestData, new Exception("UrlWhitelistRequest empty whitelist."));
      }
      catch (Exception ex)
      {
        result = new UrlWhitelistResponseData(cdsRequestData, ex);
      }

      return result;
    }
  }
}
