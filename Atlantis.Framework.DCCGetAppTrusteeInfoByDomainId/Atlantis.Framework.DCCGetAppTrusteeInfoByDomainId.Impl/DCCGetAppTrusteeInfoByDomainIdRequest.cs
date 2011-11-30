using System;
using Atlantis.Framework.DCCGetAppTrusteeInfoByDomainId.Impl.AppTrusteeInfoWS;
using Atlantis.Framework.DCCGetAppTrusteeInfoByDomainId.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetAppTrusteeInfoByDomainId.Impl
{
  public class DCCGetAppTrusteeInfoByDomainIdRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;
      string responseXml = null;

      RegCheckDomainStatusWebSvcService service = null;
      try
      {
        DCCGetAppTrusteeInfoByDomainIdRequestData request = (DCCGetAppTrusteeInfoByDomainIdRequestData)oRequestData;

        service = new RegCheckDomainStatusWebSvcService();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        responseXml = service.GetApplicationTrusteeInfoByDomainId(request.ToXML());
        result = new DCCGetAppTrusteeInfoByDomainIdResponseData(responseXml, oRequestData);
      }
      catch (Exception ex)
      {
        result = new DCCGetAppTrusteeInfoByDomainIdResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return result;
    }

    #endregion
  }
}
