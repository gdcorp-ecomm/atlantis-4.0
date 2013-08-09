using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainGetResourceDomainId.Interface
{
  public class DomainGetResourceDomainIdResponseData: IResponseData
  {
    private AtlantisException _exception;

  
    private string _billingResourceId = string.Empty;  
    public string BillingResourceId
    {
      get { return _billingResourceId; }
    }

    private string _domainId = string.Empty;  
    public string DomainId
    {
      get { return _domainId; }
    }

    public DomainGetResourceDomainIdResponseData(string domainId, string billingResourceId)
    {
      _domainId = domainId;
      _billingResourceId = billingResourceId;
    }

    public DomainGetResourceDomainIdResponseData(Exception ex, RequestData requestData)
    {
      _exception = new AtlantisException(requestData, "DomainGetResourceDomainIdResponseData", ex.Message, string.Empty, ex);
    }

    public DomainGetResourceDomainIdResponseData(AtlantisException ex)
    {
      _exception = ex;
    }

    public string ToXML()
    {
      const string xmlFormatString = "<domain_resouce_response_data><domain_id>{0}</domain_id><billing_resource_id>{1}</billing_resource_id></domain_resouce_response_data>";
      return string.Format(xmlFormatString, DomainId, BillingResourceId);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
