using System;
using Atlantis.Framework.DCCGetDomainContacts.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainContacts.Impl
{
  public class DCCGetDomainContactsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCGetDomainContactsResponseData responseData;
      string responseXml = string.Empty;
      DsWeb.RegCheckDomainStatusWebSvcService regCheckService = null;

      try
      {
        regCheckService = new DsWeb.RegCheckDomainStatusWebSvcService();
        DCCGetDomainContactsRequestData oRequest = (DCCGetDomainContactsRequestData)oRequestData;
        regCheckService.Url = ((WsConfigElement)oConfig).WSURL;
        regCheckService.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        responseXml = regCheckService.GetDomainInfoByNameWithContacts(oRequest.ToXML());
        responseData = new DCCGetDomainContactsResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCGetDomainContactsResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCGetDomainContactsResponseData(responseXml, oRequestData, ex);
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

    #endregion
  }
}
