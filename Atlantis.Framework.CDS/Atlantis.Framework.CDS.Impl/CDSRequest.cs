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
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CDSResponseData result;

      CDSRequestData cdsRequestData = (CDSRequestData)requestData;

      WsConfigElement wsConfig = (WsConfigElement)config;

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);

      try
      {
        string responseText = service.GetWebResponse();
        result = new CDSResponseData(responseText);
      }
      catch (WebException ex)
      {
        if (ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
        {
            result = new CDSResponseData(cdsRequestData, ex);
        }
        else
        {
          throw;
        }        
      }
      catch (Exception ex)
      {
        result = new CDSResponseData(cdsRequestData, ex);
      }

      return result;
    }
  }
}
