using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DCCGetExpirationCount.Interface;
using Atlantis.Framework.DCCGetExpirationCount.Impl.DomainStatusWS;

namespace Atlantis.Framework.DCCGetExpirationCount.Impl
{
  public class DCCGetExpirationCountRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result;
      string responseXml = null;
      RegCheckDomainStatusWebSvcService service = null;

      try
      {
        DCCGetExpirationCountRequestData request = (DCCGetExpirationCountRequestData)oRequestData;

        service = new RegCheckDomainStatusWebSvcService();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        responseXml = service.GetExpirationDomainCountsByShopperId(request.ToXML());
        result = new DCCGetExpirationCountResponseData(responseXml, oRequestData);
      }
      catch (Exception ex)
      {
        result = new DCCGetExpirationCountResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if(service != null)
        {
          service.Dispose();
        }
      }

      return result;
    }
  }
}
