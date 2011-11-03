using System;

using Atlantis.Framework.Interface;
using Atlantis.Framework.DCCGetAppTrusteeInfoByDomainId.Interface;
using Atlantis.Framework.DCCGetAppTrusteeInfoByDomainId.Impl.AppTrusteeInfoWS;

namespace Atlantis.Framework.DCCGetAppTrusteeInfoByDomainId.Impl
{
  public class DCCGetAppTrusteeInfoByDomainIdRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;
      string responseXml = null;

      try
      {
        DCCGetAppTrusteeInfoByDomainIdRequestData request = (DCCGetAppTrusteeInfoByDomainIdRequestData)oRequestData;

        RegCheckDomainStatusWebSvcService service = new RegCheckDomainStatusWebSvcService();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        responseXml = service.GetApplicationTrusteeInfoByDomainId(request.ToXML());
        result = new DCCGetAppTrusteeInfoByDomainIdResponseData(responseXml, oRequestData);
      }
      catch (Exception ex)
      {
        result = new DCCGetAppTrusteeInfoByDomainIdResponseData(responseXml, oRequestData, ex);
      }

      return result;
    }

    #endregion
  }
}
