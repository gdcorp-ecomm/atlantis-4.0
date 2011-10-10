using System;
using Atlantis.Framework.DCCGetNameservers.Impl.DsWeb;
using Atlantis.Framework.DCCGetNameservers.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetNameservers.Impl
{
  public class DCCGetNameserversRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCGetNameserversResponseData responseData;
      string responseXml = string.Empty;
      RegCheckDomainStatusWebSvcService regCheckService = null;

      try
      {
        regCheckService = new RegCheckDomainStatusWebSvcService();
        DCCGetNameserversRequestData oRequest = (DCCGetNameserversRequestData)oRequestData;
        regCheckService.Url = ((WsConfigElement)oConfig).WSURL;
        regCheckService.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        responseXml = regCheckService.GetNameserverInfoByDomainNameAndShopperId(oRequest.ToXML());
        responseData = new DCCGetNameserversResponseData(responseXml);
        
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCGetNameserversResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCGetNameserversResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if(regCheckService != null)
        {
          regCheckService.Dispose();
        }
      }

      return responseData;
    }
  }
}
