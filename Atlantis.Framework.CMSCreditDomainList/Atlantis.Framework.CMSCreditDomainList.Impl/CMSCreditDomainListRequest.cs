using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.CMSCreditDomainList.Interface;

namespace Atlantis.Framework.CMSCreditDomainList.Impl
{
  public class CMSCreditDomainListRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      string responseText = string.Empty;

      try
      {
        CMSCreditDomainListRequestData cmsRequest = (CMSCreditDomainListRequestData)requestData;
        cmsCreditDomainLists.CmsCreditSupport service = new cmsCreditDomainLists.CmsCreditSupport();
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)cmsRequest.RequestTimeout.TotalMilliseconds;
        responseText = service.CmsCreditDomainLists(cmsRequest.ToXML());
        result = new CMSCreditDomainListResponseData(responseText);
      }
      catch (AtlantisException aex)
      {
        result = new CMSCreditDomainListResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new CMSCreditDomainListResponseData(requestData, ex);
      }

      return result;
    }
  }
}
