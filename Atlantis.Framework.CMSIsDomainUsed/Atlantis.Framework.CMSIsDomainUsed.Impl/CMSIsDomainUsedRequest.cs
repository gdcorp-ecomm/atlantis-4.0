using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.CMSIsDomainUsed.Interface;

namespace Atlantis.Framework.CMSIsDomainUsed.Impl
{
  public class CMSIsDomainUsedRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CMSIsDomainUsedResponseData responseData = null;

      try
      {
        CMSIsDomainUsedRequestData cmsRequest = (CMSIsDomainUsedRequestData)requestData;
        ActivationService.ActivationWizardSupport service = new ActivationService.ActivationWizardSupport();
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)cmsRequest.RequestTimeout.TotalMilliseconds;
        System.Diagnostics.Debug.WriteLine(cmsRequest.ToXML());
        string responseText = service.IsDomainValidForInstantPage(cmsRequest.ToXML());
        System.Diagnostics.Debug.WriteLine(responseText);
        responseData = new CMSIsDomainUsedResponseData(responseText);
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new CMSIsDomainUsedResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new CMSIsDomainUsedResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
