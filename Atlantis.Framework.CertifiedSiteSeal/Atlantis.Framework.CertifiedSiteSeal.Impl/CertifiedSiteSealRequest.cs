using Atlantis.Framework.CertifiedSiteSeal.Impl.CertifiedDomainsWS;
using Atlantis.Framework.CertifiedSiteSeal.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Net;

namespace Atlantis.Framework.CertifiedSiteSeal.Impl
{
  public class CertifiedSiteSealRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      try
      {
        CertifiedSiteSealRequestData siteSealRequestData = (CertifiedSiteSealRequestData)oRequestData;
        CDWhoisWebSvc cdWhoisWebSvc = new CDWhoisWebSvc();
        cdWhoisWebSvc.Url = ((WsConfigElement)oConfig).WSURL;
        cdWhoisWebSvc.Timeout = siteSealRequestData.Timeout;
        cdWhoisWebSvc.PreAuthenticate = true;
        cdWhoisWebSvc.Credentials = (ICredentials)new NetworkCredential(siteSealRequestData.UserID, siteSealRequestData.UserPwd);
        return (IResponseData)new CertifiedSiteSealResponseData(cdWhoisWebSvc.GetSiteSealHtmlForDomain(siteSealRequestData.App, siteSealRequestData.Domain).Results);
      }
      catch (Exception ex)
      {
        return (IResponseData)new CertifiedSiteSealResponseData(oRequestData, ex);
      }
    }
  }
}
