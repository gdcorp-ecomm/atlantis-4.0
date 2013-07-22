using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.CDS.Impl
{
  public class ContentVersionRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ContentVersionResponseData result;

      CDSRequestData cdsRequestData = (CDSRequestData)requestData;

      WsConfigElement wsConfig = (WsConfigElement)config;

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);
      
      try
      {
        string responseText = service.GetWebResponse();

        result = !string.IsNullOrEmpty(responseText) ? new ContentVersionResponseData(responseText) : new ContentVersionResponseData(cdsRequestData, new Exception("Empty response from the CDS service."));
      }
      catch (Exception ex)
      {
        result = new ContentVersionResponseData(cdsRequestData, ex);
      }

      return result;
    }
  }
}
