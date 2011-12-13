using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.CMSDomainAvail.Interface;


namespace Atlantis.Framework.CMSDomainAvail.Impl
{
  public class CMSDomainAvailRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      string responseText = string.Empty;

      try
      {
        CMSDomainAvailRequestData cmsRequest = (CMSDomainAvailRequestData)requestData;
        cmsValidateDomain.CmsCreditSupport service = new cmsValidateDomain.CmsCreditSupport();
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)cmsRequest.RequestTimeout.TotalMilliseconds;
        System.Diagnostics.Debug.WriteLine(cmsRequest.ToXML());
        responseText = service.IsDomainValidForInstantPage(cmsRequest.ToXML());
        System.Diagnostics.Debug.WriteLine(responseText);
        result = new CMSDomainAvailResponseData(responseText);
      }
      catch (AtlantisException aex)
      {
        result = new CMSDomainAvailResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new CMSDomainAvailResponseData(requestData, ex);
      }

      return result;
    }
  }
}
