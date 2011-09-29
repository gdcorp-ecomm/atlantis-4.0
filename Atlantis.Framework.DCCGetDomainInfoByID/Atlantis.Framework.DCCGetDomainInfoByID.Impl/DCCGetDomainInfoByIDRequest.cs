using System;
using Atlantis.Framework.DCCGetDomainInfoByID.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainInfoByID.Impl
{
  public class DCCGetDomainInfoByIDRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCGetDomainInfoByIDResponseData responseData;
      string responseXml = string.Empty;
      DsWebGet.RegCheckDomainStatusWebSvcService regCheckService = null;

      try
      {
        regCheckService = new DsWebGet.RegCheckDomainStatusWebSvcService();
        DCCGetDomainInfoByIDRequestData oRequest = (DCCGetDomainInfoByIDRequestData)oRequestData;
        regCheckService.Url = ((WsConfigElement)oConfig).WSURL;
        regCheckService.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        responseXml = regCheckService.GetDomainInfoByID(oRequest.ToXML());
        responseData = new DCCGetDomainInfoByIDResponseData(responseXml, oRequest);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCGetDomainInfoByIDResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCGetDomainInfoByIDResponseData(responseXml, oRequestData, ex);
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
