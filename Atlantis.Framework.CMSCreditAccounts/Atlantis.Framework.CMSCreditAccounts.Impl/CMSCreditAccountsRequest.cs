using System;
using Atlantis.Framework.CMSCreditAccounts.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CMSCreditAccounts.Impl
{
  public class CMSCreditAccountsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      string responseText = string.Empty;
      
      ActivationService.ActivationWizardSupport service = new ActivationService.ActivationWizardSupport();
      
      try
      {
        CMSCreditAccountsRequestData cmsRequest = (CMSCreditAccountsRequestData)requestData;
        
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)cmsRequest.RequestTimeout.TotalMilliseconds;
        responseText = service.DomainLists(cmsRequest.ToXML());
        result = new CMSCreditAccountsResponseData(responseText);
      }
      catch (AtlantisException aex)
      {
        result = new CMSCreditAccountsResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new CMSCreditAccountsResponseData(requestData, ex);
      }
      finally
      {
        service.Dispose();
      }

      return result;
    }
  }
}
